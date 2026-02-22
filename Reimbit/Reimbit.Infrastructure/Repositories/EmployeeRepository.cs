using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.Employee;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Models;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class EmployeeRepository(
    IApplicationDbContext context
) : IEmployeeRepository
{
    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> Insert(EmployeeInsertRequest request)
    {
        var dbContext = (DbContext)context;

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var user = new SecUser
            {
                OrganizationId = request.OrganizationId,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                DisplayName = request.DisplayName.Trim(),
                Email = request.Email,
                MobileNo = request.MobileNo,
                ProfileImageUrl = request.ProfileImageUrl,
                IsActive = request.IsActive,
                Created = request.Created,
                CreatedByUserId = request.CreatedByUserId
            };

            context.SecUsers.Add(user);
            await context.SaveChangesAsync();

            var userRole = new SecUserRole
            {
                UserId = user.UserId,
                RoleId = (int)request.RoleId,
                CreatedByUserId = request.CreatedByUserId,
                Created = request.Created,
            };

            context.SecUserRoles.Add(userRole);

            if (request.ManagerId != null)
            {
                var managerId = (int)request.ManagerId.Value;

                var managerExists = await context.SecUsers
                    .AsNoTracking()
                    .AnyAsync(x => x.UserId == managerId && x.OrganizationId == request.OrganizationId);

                if (!managerExists)
                {
                    return Error.Validation("Manager.NotFound", "Manager not found in organization.");
                }

                var now = DateTime.UtcNow;
                var validFrom = request.ManagerValidFrom ?? now;

                var mapping = new SecUserManager
                {
                    UserId = user.UserId,
                    ManagerId = managerId,
                    ManagerType = request.ManagerType ?? (byte)1,
                    IsPrimary = request.IsPrimaryManager,
                    ValidFrom = validFrom,
                    ValidTo = request.ManagerValidTo
                };

                context.SecUserManagers.Add(mapping);
            }

            var rowsAffected = await context.SaveChangesAsync();
            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt>()
            {
                Id = user.UserId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();

            return Error.Failure("Employee.Insert.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> AssignEmployeesToManager(AssignEmployeesToManagerRequest request)
    {
        var dbContext = (DbContext)context;
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        try
        {
            if (request.EmployeeIds.Count == 0)
            {
                return Error.Validation("EmployeeIds.Required", "EmployeeIds is required.");
            }

            var managerId = (int)request.ManagerId;
            var employeeIds = request.EmployeeIds.Select(x => (int)x).Distinct().ToList();

            if (employeeIds.Contains(managerId))
            {
                return Error.Validation("ManagerMapping.Invalid", "Manager cannot be assigned as their own employee.");
            }

            var managerExists = await context.SecUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserId == managerId && x.OrganizationId == request.OrganizationId);

            if (!managerExists)
            {
                return Error.NotFound("Manager.NotFound", "Manager not found.");
            }

            var employeesInOrg = await context.SecUsers
                .AsNoTracking()
                .Where(x => x.OrganizationId == request.OrganizationId && employeeIds.Contains(x.UserId))
                .Select(x => x.UserId)
                .ToListAsync();

            if (employeesInOrg.Count != employeeIds.Count)
            {
                return Error.Validation("Employees.NotFound", "One or more employees were not found in organization.");
            }

            var now = DateTime.UtcNow;
            var validFrom = request.ValidFrom ?? now;

            foreach (var employeeId in employeeIds)
            {
                var activeMappings = await context.SecUserManagers
                    .Where(m =>
                        m.UserId == employeeId &&
                        m.ManagerType == request.ManagerType &&
                        (m.ValidTo == null || m.ValidTo > now))
                    .ToListAsync();

                foreach (var existing in activeMappings)
                {
                    existing.ValidTo = now;
                }

                var mapping = new SecUserManager
                {
                    UserId = employeeId,
                    ManagerId = managerId,
                    ManagerType = request.ManagerType,
                    IsPrimary = request.IsPrimary,
                    ValidFrom = validFrom,
                    ValidTo = request.ValidTo
                };

                context.SecUserManagers.Add(mapping);
            }

            var rowsAffected = await context.SaveChangesAsync();
            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt>
            {
                Id = managerId,
                RowsAffected = rowsAffected
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("UserManager.Assign.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<EmployeeSelectPageResponse>>> List(int organizationId)
    {
        var data = context.SecUsers
            .AsNoTracking()
            .Include(u => u.SecUserRoleUsers)
            .Where(u => u.OrganizationId == organizationId)
            .Select(u => new EmployeeSelectPageResponse
            {
                UserId = u.UserId,
                DisplayName = u.DisplayName,
                Email = u.Email,
                MobileNo = u.MobileNo ?? string.Empty,
                Role = u.SecUserRoleUsers
                    .Join(context.SecRoles,
                        ur => ur.RoleId,
                        r => r.RoleId,
                        (ur, r) => r.RoleName)
                    .FirstOrDefault() ?? string.Empty,
                IsActive = u.IsActive
            }).ToList();


        return new PagedResult<EmployeeSelectPageResponse>
        {
            Total = data.Count,
            Data = data
        };
    }

    public async Task<ErrorOr<EmployeeSelecttViewResponse>> View(EncryptedInt userId)
    {
        var data = await context.SecUsers
            .AsNoTracking()
            .Where(u => u.UserId == (int)userId)
            .Select(u => new EmployeeSelecttViewResponse
            {
                Name = u.DisplayName,
                Role = u.SecUserRoleUsers
                    .Join(context.SecRoles,
                        ur => ur.RoleId,
                        r => r.RoleId,
                        (ur, r) => r.RoleName)
                    .FirstOrDefault(),
                TotalExpense = u.ExpExpenseEmployees.Sum(e => e.Amount),
                Created = u.Created,
                CreatedByUserName = u.CreatedByUser != null ? u.CreatedByUser.DisplayName : null,
                Modified = u.Modified,
                ModifiededByUserName = u.ModifiedByUser != null ? u.ModifiedByUser.DisplayName : null
            })
            .FirstOrDefaultAsync();

        return data;
    }
}