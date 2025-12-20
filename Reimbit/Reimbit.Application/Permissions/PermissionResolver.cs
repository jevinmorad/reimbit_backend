using Common.Security;
using System.Security.Claims;

namespace Reimbit.Application.Permissions;

public class PermissionResolver(ICurrentUserProvider currentUserProvider) : IPermissionResolver
{
    //private readonly CurrentUser<TokenData> currentUser = currentUserProvider.GetCurrentUser<TokenData>();

    public Task<bool> HasPermission(string userName, string permission, IEnumerable<Claim> claims)
    {
        throw new NotImplementedException();
    }
}
