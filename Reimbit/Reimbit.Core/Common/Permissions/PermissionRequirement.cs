using Microsoft.AspNetCore.Authorization;

namespace Reimbit.Core.Common.Permissions;

public sealed class PermissionRequirement(Permission permission) : IAuthorizationRequirement
{
    public Permission Permission { get; } = permission;
}