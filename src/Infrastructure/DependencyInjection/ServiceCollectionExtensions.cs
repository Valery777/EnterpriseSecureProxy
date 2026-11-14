using EnterpriseSecureProxy.src.Application.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Http;
using Microsoft.Extensions.DependencyInjection;
using EnterpriseSecureProxy.src.Infrastructure.Repositories;
namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IOperationRepository, OperationRepository>();
      
        services.AddScoped<IDestinationRepository, DestinationRepository>();
        
        services.AddScoped<IProxyHttpForwarder, ProxyHttpForwarder>();

        services.AddScoped<IAuditRepository, AuditRepository>();

        return services;
    }
}
