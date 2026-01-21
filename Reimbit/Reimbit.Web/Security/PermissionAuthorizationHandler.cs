using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Reimbit.Core.Common.Permissions;
using Reimbit.Domain.Interfaces;

namespace Reimbit.Web.Security;

public class PermissionAuthorizationHandler(
    IApplicationDbContext dbContext,
    ICurrentUserProvider currentUserProvider
) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userId = currentUserProvider.GetUserId();
        if (userId == 0)
        {
            context.Fail();
            return;
        }

        var now = DateTime.UtcNow;

        var hasPermission = await dbContext.SecUserRoles
            .AsNoTracking()
            .AnyAsync(ur =>
                ur.UserId == userId &&
                ur.Role.ValidFrom <= now && 
                (ur.Role.ValidTo == null || ur.Role.ValidTo > now) && 
                ur.Role.IsActive &&
                ur.Role.SecRoleClaims.Any(rc => rc.ClaimValue == (int)requirement.Permission)
            );

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}