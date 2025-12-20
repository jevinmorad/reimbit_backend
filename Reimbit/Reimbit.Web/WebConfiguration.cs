using Common.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reimbit.Application.Permissions;
using Reimbit.Web.Security;

namespace Reimbit.Web;

public static class WebConfiguration
{
    public static IServiceCollection AddWebComponentsService(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        services.AddScoped<IPermissionResolver, PermissionResolver>();

        return services;
    }
}
