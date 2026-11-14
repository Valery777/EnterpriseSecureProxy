using Application.Services;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Application.Services;

namespace Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IOperationResolver, OperationResolver>();

        services.AddScoped<IDestinationResolver, DestinationResolver>();

        services.AddScoped<IParameterValidator, ParameterValidator>();
            
        services.AddScoped<IHeaderFilter, HeaderFilter>();
        
        services.AddScoped<IProxyExecutionService, ProxyExecutionService>();
        
        services.AddScoped<IAuditService, AuditService>();

        return services;
    }
}