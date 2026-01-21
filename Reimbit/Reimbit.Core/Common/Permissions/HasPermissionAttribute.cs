using Microsoft.AspNetCore.Authorization;

namespace Reimbit.Core.Common.Permissions;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    private const string POLICY_PREFIX = "Permission:";

    public HasPermissionAttribute(Permission permission)
    {
        Policy = $"{POLICY_PREFIX}{(int)permission}";
    }
}