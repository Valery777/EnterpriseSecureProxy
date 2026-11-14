using EnterpriseSecureProxy.src.Domain.Common;

namespace EnterpriseSecureProxy.src.Domain.Entities
{
    public sealed class ConfigurationHistory : BaseEntity
    {
        public string EntityType { get; set; } = string.Empty;

        public Guid EntityId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string? ChangeSummary { get; set; }

        public string? ChangedBy { get; set; }

        public DateTime ChangedUtc { get; set; }
    }
}
