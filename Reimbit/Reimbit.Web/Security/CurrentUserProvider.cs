using Common.Security;
using Microsoft.AspNetCore.Http;
using Reimbit.Core.Models;
using System.Security.Claims;

namespace Reimbit.Web.Security;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public int GetUserId()
    {
        var userIdClaim = User?.FindFirstValue("userId");
        return int.TryParse(userIdClaim, out int userId) ? userId : 0;
    }

    public int GetOrganizationId()
    {
        var orgIdClaim = User?.FindFirstValue("orgId");
        return int.TryParse(orgIdClaim, out int orgId) ? orgId : 0;
    }

    public int? GetUserRoleId()
    {
        var roleIdClaim = User?.FindFirstValue(ClaimTypes.Role);
        return int.TryParse(roleIdClaim, out int roleId) ? roleId : null;
    }

    public string GetUserEmail()
    {
        return User?.FindFirstValue(ClaimTypes.Email)!;
    }

    public CurrentUser<TokenData> GetCurrentUser<TModel>()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity?.IsAuthenticated == true)
            return new CurrentUser<TokenData>(default!);

        var tokenData = new TokenData
        {
            UserId = int.Parse(user.FindFirstValue("userId") ?? "0"),
            OrganizationId = int.Parse(user.FindFirstValue("orgId") ?? "0"),
            Email = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            RoleId = int.Parse(user.FindFirstValue(ClaimTypes.Role) ?? "0")
        };

        return new CurrentUser<TokenData>(tokenData);
    }
}