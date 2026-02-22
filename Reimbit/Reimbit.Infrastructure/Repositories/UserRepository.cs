using AegisInt.Core;
using Common.Data.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Reimbit.Contracts.User;
using Reimbit.Core.Common.Permissions;
using Reimbit.Domain.Interfaces;
using Reimbit.Domain.Repositories;

namespace Reimbit.Infrastructure.Repositories;

public class UserRepository(IApplicationDbContext context) : IUserRepository
{
    public async Task<ErrorOr<UserResponse>> GetProfile(int userId)
    {
        var profile = await context.SecUsers
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .Select(u => new UserResponse
            {
                UserId = u.UserId,
                DisplayName = u.DisplayName,
                Email = u.Email,
                MobileNo = u.MobileNo,
                ProfileImageUrl = u.ProfileImageUrl,
                IsActive = u.IsActive,
                Created = u.Created,
                Modified = u.Modified
            })
            .FirstOrDefaultAsync();

        if (profile == null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        return profile;
    }

    public async Task<ErrorOr<OperationResponse<EncryptedInt>>> UpdateProfile(UpdateProfileRequest request)
    {
        var db = (DbContext)context;
        await using var tx = await db.Database.BeginTransactionAsync();

        try
        {
            var user = await context.SecUsers.FirstOrDefaultAsync(u => u.UserId == request.UserId);
            if (user == null)
            {
                return Error.NotFound("User.NotFound", "User not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.DisplayName))
            {
                user.DisplayName = request.DisplayName.Trim();
            }

            user.MobileNo = request.MobileNo;
            user.ProfileImageUrl = request.ProfileImageUrl;
            user.Modified = request.Modified ?? DateTime.UtcNow;
            user.ModifiedByUserId = request.ModifiedByUserId;

            var rows = await context.SaveChangesAsync();
            await tx.CommitAsync();

            return new OperationResponse<EncryptedInt>
            {
                Id = user.UserId,
                RowsAffected = rows
            };
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return Error.Failure("User.Update.Failed", ex.Message);
        }
    }

    public async Task<ErrorOr<PermissionsResponse>> GetPermissions(int userId)
    {
        var roleIds = await context.SecUserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .Distinct()
            .ToListAsync();

        var claimValues = roleIds.Count == 0
            ? new List<int>()
            : await context.SecRoleClaims
                .AsNoTracking()
                .Where(rc => roleIds.Contains(rc.RoleId))
                .Select(rc => rc.ClaimValue)
                .ToListAsync();

        var dict = Enum.GetValues(typeof(Permission))
            .Cast<Permission>()
            .ToDictionary(p => p.ToString(), p => claimValues.Contains((int)p));

        return new PermissionsResponse { Permissions = dict };
    }

    public async Task<ErrorOr<InfoResponse>> GetInfo(int userId)
    {
        var user = await context.SecUsers
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .Select(u => new UserResponse
            {
                UserId = u.UserId,
                DisplayName = u.DisplayName,
                Email = u.Email,
                MobileNo = u.MobileNo,
                ProfileImageUrl = u.ProfileImageUrl,
                IsActive = u.IsActive,
                Created = u.Created,
                Modified = u.Modified
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        var roleIds = await context.SecUserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .Distinct()
            .ToListAsync();

        var claimValues = roleIds.Count == 0
            ? new List<int>()
            : await context.SecRoleClaims
                .AsNoTracking()
                .Where(rc => roleIds.Contains(rc.RoleId))
                .Select(rc => rc.ClaimValue)
                .ToListAsync();

        var permissions = Enum.GetValues(typeof(Permission))
            .Cast<Permission>()
            .ToDictionary(p => p.ToString(), p => claimValues.Contains((int)p));

        var info = new InfoResponse
        {
            User = user,
            Permissions = permissions
        };

        return info;
    }
}