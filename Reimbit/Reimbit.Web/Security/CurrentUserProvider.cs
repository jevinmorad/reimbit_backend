using Common.Security;
using Microsoft.AspNetCore.Http;
using Reimbit.Core.Models;
using System.Security.Claims;

namespace Reimbit.Web.Security;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public int GetUserId()
    {
        var userIdClaim = User?.FindFirstValue("userId");
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    public int GetOrganizationId()
    {
        var organizationIdClaim = User?.FindFirstValue("organizationId");
        return int.TryParse(organizationIdClaim, out var organizationId) ? organizationId : 0;
    }

    public int? GetUserRoleId()
    {
        var roleIdClaim = User?.FindFirstValue("roleId");
        return int.TryParse(roleIdClaim, out var roleId) ? roleId : null;
    }

    public string GetUserEmail()
    {
        return User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    }

    public CurrentUser<TokenData> GetCurrentUser<TModel>()
    {
        var user = httpContextAccessor.HttpContext?.User;

        //if (user == null || user.Identity is not { IsAuthenticated: true })
        //{
        //    return new CurrentUser<TokenData>(default, default, default!);
        //}

        var tokenData = new TokenData
        {
            UserId = int.TryParse(user?.FindFirstValue("userId"), out var userId)
                ? userId
                : 0,
            OrganizationId = int.TryParse(user?.FindFirstValue("organizationId"), out var organizationId)
                ? organizationId
                : 0,
            Email = user?.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            RoleId = int.TryParse(user?.FindFirstValue("roleId"), out var roleId)
                ? roleId
                : 0
        };

        return new CurrentUser<TokenData>(tokenData.UserId, tokenData.OrganizationId, tokenData);
    }
}