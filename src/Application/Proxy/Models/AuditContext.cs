namespace EnterpriseSecureProxy.src.Application.Proxy.Models
{
    public sealed class AuditContext
    {
        public string CorrelationId { get; set; } = string.Empty;

        public string OperationKey { get; set; } = string.Empty;

        public Guid? DestinationId { get; set; }

        public DateTime StartedUtc { get; set; }

        public DateTime CompletedUtc { get; set; }

        public long DurationMs { get; set; }
    }
}
