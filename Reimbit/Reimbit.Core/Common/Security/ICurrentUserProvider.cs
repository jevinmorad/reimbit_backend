using Reimbit.Core.Models;

namespace Common.Security;

public interface ICurrentUserProvider
{
    public int GetUserId();

    public int GetOrganizationId();

    public int? GetUserRoleId();

    public string GetUserEmail();

    public CurrentUser<TokenData> GetCurrentUser<TModel>();
}
