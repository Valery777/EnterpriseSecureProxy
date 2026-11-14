using System.Text.Json;

namespace EnterpriseSecureProxy.src.Application.Proxy.DTO
{
    public sealed class ExecuteProxyRequest
    {
        public string ClientId { get; set; } = string.Empty;

        public JsonElement Payload { get; set; }

        public EncryptedPayload? EncryptedPayload { get; set; }
    }
}
