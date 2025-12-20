using System.Security.Claims;

namespace Common.Security;

public interface IPermissionResolver
{
    Task<bool> HasPermission(string userName, string permission, IEnumerable<Claim> claims);
}
