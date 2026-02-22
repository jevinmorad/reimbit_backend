using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.Role;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class RoleRepository(IApplicationDbContext context) : IRoleRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(RoleInsertRequest request)
    {
        var db = (DbContext)context;
        await using var tx = await db.Database.BeginTransactionAsync();

        try
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
            {
                return Error.Validation("Role.RoleName.Required", "RoleName is required.");
            }

            var roleNameNormalized = request.RoleName.Trim();
            
            var roleExists = await context.SecRoles.AnyAsync(x =>
                x.OrganizationId == request.OrganizationID &&
                x.RoleName == roleNameNormalized);

            if (roleExists)
            {
                return Error.Conflict("Role.Duplicate", "Role with this name already exists in the organization.");
            }

            var role = new SecRole
            {
                RoleName = roleNameNormalized,
                Description = request.Description?.Trim(),
                OrganizationId = request.OrganizationID,
                CreatedByUserId = request.CreatedByUserID,
                ModifiedByUserId = request.CreatedByUserID,
                Created = request.Created,
                IsActive = request.IsActive,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo
            };

            await context.SecRoles.AddAsync(role);
            await context.SaveChangesAsync();

            if (request.PermissionValues != null && request.PermissionValues.Count > 0)
            {
                foreach (var claimValue in request.PermissionValues.Distinct())
                {
                    var rc = new SecRoleClaim
                    {
                        RoleId = role.RoleId,
                        ClaimValue = claimValue,
                        CreatedByUserId = request.CreatedByUserID,
                        Created = request.Created
                    };
                    await context.SecRoleClaims.AddAsync(rc);
                }
                await context.SaveChangesAsync();
            }

            if (request.Assignments != null && request.Assignments.Count > 0)
            {
                foreach (var a in request.Assignments)
                {
                    var assignment = new SecUserRole
                    {
                        UserId = a.UserId,
                        RoleId = role.RoleId,
                        CreatedByUserId = a.CreatedByUserId,
                        Created = request.Created,
                    };

                    await context.SecUserRoles.AddAsync(assignment);
                }
                await context.SaveChangesAsync();
            }

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt>
            {
                Id = role.RoleId,
                RowsAffected = 1
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Role.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Update(RoleUpdateRequest request)
    {
        var db = (DbContext)context;
        await using var tx = await db.Database.BeginTransactionAsync();

        try
        {
            var role = await context.SecRoles
                .Include(r => r.SecRoleClaims)
                .FirstOrDefaultAsync(r => r.RoleId == request.RoleID);

            if (role == null)
            {
                return Error.NotFound("Role.NotFound", "Role not found.");
            }

            role.RoleName = !string.IsNullOrWhiteSpace(request.RoleName) ? request.RoleName.Trim() : role.RoleName;
            role.Description = request.Description?.Trim() ?? role.Description;
            role.ModifiedByUserId = request.ModifiedByUserID;
            role.Modified = request.Modified;
            role.IsActive = request.IsActive;

            if (request.PermissionValues != null)
            {
                var existingClaims = role.SecRoleClaims.ToList();
                var existingValues = existingClaims.Select(rc => rc.ClaimValue).ToHashSet();
                
                var newValues = request.PermissionValues.Distinct().ToHashSet();

                var toAdd = newValues.Except(existingValues).ToList();
                var toRemove = existingClaims.Where(rc => !newValues.Contains(rc.ClaimValue)).ToList();

                if (toRemove.Count > 0)
                {
                    context.SecRoleClaims.RemoveRange(toRemove);
                }

                foreach (var val in toAdd)
                {
                    var rc = new SecRoleClaim
                    {
                        RoleId = role.RoleId,
                        ClaimValue = val,
                        CreatedByUserId = request.ModifiedByUserID,
                        Created = request.Modified
                    };
                    await context.SecRoleClaims.AddAsync(rc);
                }
            }

            await context.SaveChangesAsync();
            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt>
            {
                Id = role.RoleId,
                RowsAffected = 1
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Role.Update.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Delete(EncryptedInt roleId)
    {
        var db = (DbContext)context;
        await using var tx = await db.Database.BeginTransactionAsync();

        try
        {
            var now = DateTime.UtcNow;
            var id = (int)roleId;
            var role = await context.SecRoles.FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
            {
                return Error.NotFound("Role.NotFound", "Role not found.");
            }

            // Check for assignments (any assignment is now effectively active if role is active)
            var hasAssignments = await context.SecUserRoles
                .AsNoTracking()
                .AnyAsync(ur => ur.RoleId == id);

            if (hasAssignments)
            {
                // Soft delete
                role.IsActive = false;
                role.Modified = now;
                await context.SaveChangesAsync();
            }
            else
            {
                // Hard delete
                var claims = await context.SecRoleClaims.Where(rc => rc.RoleId == id).ToListAsync();
                context.SecRoleClaims.RemoveRange(claims);

                var assignments = await context.SecUserRoles.Where(ur => ur.RoleId == id).ToListAsync();
                context.SecUserRoles.RemoveRange(assignments);

                context.SecRoles.Remove(role);
                await context.SaveChangesAsync();
            }

            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt>
            {
                Id = role.RoleId,
                RowsAffected = 1
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Role.Delete.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<RoleSelectPKResponse>> Get(EncryptedInt roleId)
    {
        var role = await context.SecRoles
            .AsNoTracking()
            .Where(r => r.RoleId == (int)roleId)
            .Select(r => new RoleSelectPKResponse
            {
                RoleID = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description
            })
            .FirstOrDefaultAsync();

        if (role == null)
        {
            return Error.NotFound("Role.NotFound", "Role not found.");
        }

        return role;
    }

    public async Task<ErrorOr<RoleSelectViewResponse>> View(EncryptedInt roleId)
    {
        var role = await context.SecRoles
            .AsNoTracking()
            .Include(r => r.CreatedByUser)
            .Include(r => r.ModifiedByUser)
            .Where(r => r.RoleId == (int)roleId)
            .Select(r => new RoleSelectViewResponse
            {
                RoleName = r.RoleName,
                Description = r.Description,
                TotalUserCount = r.SecUserRoles.Count(),
                ActiveUserCount = r.SecUserRoles.Count(ur => ur.User.IsActive),
                InactiveUserCount = r.SecUserRoles.Count(ur => !ur.User.IsActive),
                Created = r.Created,
                Modified = r.Modified,
                CreatedByUserName = r.CreatedByUser.DisplayName,
                ModifiedByUserName = r.ModifiedByUser.DisplayName
            })
            .FirstOrDefaultAsync();

        if (role == null)
        {
            return Error.NotFound("Role.NotFound", "Role not found.");
        }

        return role;
    }

    public async Task<ErrorOr<PagedResult<RoleSelectPaegResponse>>> List(int organizationId)
    {
        var roles = await context.SecRoles
            .AsNoTracking()
            .Where(r => r.OrganizationId == organizationId)
            .OrderBy(r => r.RoleName)
            .Select(r => new RoleSelectPaegResponse
            {
                RoleID = r.RoleId,
                RoleName = r.RoleName,
                TotalUserCount = r.SecUserRoles.Count(),
                ActiveUserCount = r.SecUserRoles.Count(ur => ur.User.IsActive),
                InactiveUserCount = r.SecUserRoles.Count(ur => !ur.User.IsActive)
            })
            .ToListAsync();

        return new PagedResult<RoleSelectPaegResponse>
        {
            Data = roles,
            Total = roles.Count
        };
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignRoleToUser(UserRoleAssignmentRequest request)
    {
        var db = (DbContext)context;
        await using var tx = await db.Database.BeginTransactionAsync();

        try
        {
            var now = DateTime.UtcNow;

            var roleExists = await context.SecRoles
                .AsNoTracking()
                .AnyAsync(r => r.RoleId == request.RoleId && r.IsActive);

            if (!roleExists)
            {
                return Error.NotFound("Role.NotFound", "Role not found or inactive.");
            }

            var userExists = await context.SecUsers
                .AsNoTracking()
                .AnyAsync(u => u.UserId == request.UserId);

            if (!userExists)
            {
                return Error.NotFound("User.NotFound", "User not found.");
            }

            var assignmentExists = await context.SecUserRoles
                .AnyAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId);

            if (assignmentExists)
            {
                 return new OperationResponse<EncryptedInt> { Id = 0, RowsAffected = 0 }; 
            }

            var newAssignment = new SecUserRole
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
                CreatedByUserId = request.CreatedByUserId,
                Created = request.Created,
            };

            await context.SecUserRoles.AddAsync(newAssignment);
            var rows = await context.SaveChangesAsync();
            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt>
            {
                Id = newAssignment.UserRoleId,
                RowsAffected = rows
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("Role.Assign.Failed", ex.Message);
        }
    }
}