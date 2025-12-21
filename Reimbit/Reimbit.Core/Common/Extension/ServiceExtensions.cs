using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Common.Extension;

public static class ServiceExtensions
{
    private const string RootNamespace = "Reimbit.";

    public static IServiceCollection AddSimpleDomain(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly
            .GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Result", StringComparison.Ordinal));

        foreach (var type in types)
        {
            services.TryAdd(new ServiceDescriptor(type, type, ServiceLifetime.Transient));
        }

        return services;
    }

    public static IServiceCollection AddSimpleApplication(this IServiceCollection services, Assembly assembly)
    {
        RegisterAllProjectInterfacesAsScoped(services, assembly);
        return services;
    }

    public static IServiceCollection AddSimpleInfrastructure(this IServiceCollection services, Assembly assembly)
    {
        RegisterAllProjectInterfacesAsScoped(services, assembly);
        return services;
    }

    public static IServiceCollection AddSimpleWebComponents(this IServiceCollection services, Type anyTypeInWebAssembly)
    {
        services.AddValidatorsFromAssemblyContaining(anyTypeInWebAssembly);
        return services;
    }

    private static void RegisterAllProjectInterfacesAsScoped(IServiceCollection services, Assembly assembly)
    {
        var implTypes = assembly
            .GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract);

        foreach (var impl in implTypes)
        {
            var serviceInterfaces = impl
                .GetInterfaces()
                .Where(i =>
                    i.IsPublic &&
                    i.Namespace is not null &&
                    i.Namespace.StartsWith(RootNamespace, StringComparison.Ordinal));

            foreach (var serviceType in serviceInterfaces)
            {
                services.TryAddScoped(serviceType, impl);
            }
        }
    }

    public static IServiceCollection AddValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var validators =
            from type in assembly.GetExportedTypes()
            where type.IsClass && !type.IsAbstract
            from i in type.GetInterfaces()
            where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)
            select (Service: i, Impl: type);

        foreach (var (service, impl) in validators)
        {
            services.TryAddTransient(service, impl);
        }

        return services;
    }
}