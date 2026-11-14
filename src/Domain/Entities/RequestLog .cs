using EnterpriseSecureProxy.src.Domain.Common;
using EnterpriseSecureProxy.src.Domain.Enums;

namespace EnterpriseSecureProxy.src.Domain.Entities
{
    public sealed class RequestLog : BaseEntity
    {
        public string CorrelationId { get; set; } = string.Empty;

        public string OperationKey { get; set; } = string.Empty;

        public Guid? DestinationId { get; set; }

        public bool Forwarded { get; set; }

        public DateTime StartedUtc { get; set; }

        public DateTime CompletedUtc { get; set; }

        public long DurationMs { get; set; }

        public int? UpstreamStatusCode { get; set; }

        public int ProxyStatusCode { get; set; }

        public RequestOutcome Outcome { get; set; }

        public string? RejectionReason { get; set; }
    }
}
