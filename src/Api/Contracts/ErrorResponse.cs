namespace EnterpriseSecureProxy.src.Api.Contracts
{
    public sealed class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string CorrelationId { get; set; } = string.Empty;
    }
}