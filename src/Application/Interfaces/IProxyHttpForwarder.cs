using EnterpriseSecureProxy.src.Domain.Entities;
using System.Text.Json;

namespace EnterpriseSecureProxy.src.Application.Interfaces
{
    public interface IProxyHttpForwarder
    {
        Task<HttpResponseMessage> ForwardAsync(
            Operation operation,
            Destination destination,
            JsonElement payload,
            IDictionary<string, string> headers,
            CancellationToken cancellationToken);
    }
}
