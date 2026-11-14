using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Application.Security;
using EnterpriseSecureProxy.src.Infrastructure.Repositories;
using EnterpriseSecureProxy.src.Infrastructure.Secrets;
using EnterpriseSecureProxy.src.Infrastructure.Security;
using Infrastructure.Http;
using Infrastructure.Repositories;

namespace EnterpriseSecureProxy.src.Infrastructure.DependencyInjection
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // existing registrations

            services.AddScoped<ISecretProvider,
                LocalSecretProvider>();

            services.AddScoped<IEncryptionService,
                AesGcmEncryptionService>();

            services.AddScoped<IOperationRepository,
                 OperationRepository>();

            services.AddScoped<IDestinationRepository,
                DestinationRepository>();

            services.AddScoped<IAuditRepository,
                AuditRepository>();
            services.AddScoped<IProxyHttpForwarder,
                ProxyHttpForwarder>();

            return services;
        }
    }
}