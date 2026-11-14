namespace EnterpriseSecureProxy.src.Application.Proxy.DTO
{
    public sealed class ProxyExecutionResult
    {
        public int StatusCode { get; set; }

        public string? ContentType { get; set; }

        public Stream? ResponseStream { get; set; }
    }
}
