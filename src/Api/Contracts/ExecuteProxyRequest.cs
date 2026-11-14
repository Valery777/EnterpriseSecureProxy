using System.Text.Json;

namespace EnterpriseSecureProxy.src.Api.Contracts
{
    public sealed class ExecuteProxyRequest
    {
        public string ClientId { get; set; } = string.Empty;

        public JsonElement Payload { get; set; }
    }
}
