using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Reimbit.Core.Common.Permissions;

namespace Reimbit.Web.Security;

public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : IAuthorizationPolicyProvider
{
    private const string POLICY_PREFIX = "Permission:";
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider = new(options);

    public Task<AuthorizationPolicy?> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
        {
            var raw = policyName[POLICY_PREFIX.Length..];
            if (int.TryParse(raw, out var intVal) && Enum.IsDefined(typeof(Permission), intVal))
            {
                var permission = (Permission)intVal;
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(permission));
                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }
        }

        return _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}