using EnterpriseSecureProxy.src.Application.Proxy.DTO;

namespace EnterpriseSecureProxy.src.Application.Interfaces
{
    public interface IProxyExecutionService
    {
        Task<ProxyExecutionResult> ExecuteAsync(
            ExecuteProxyRequest request,
            IHeaderDictionary headers,
            CancellationToken cancellationToken);
    }
}
