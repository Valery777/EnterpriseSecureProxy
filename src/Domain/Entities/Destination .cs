using EnterpriseSecureProxy.src.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseSecureProxy.src.Domain.Entities
{
    public sealed class Destination : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string BaseUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int TimeoutSeconds { get; set; }

        public string? AuthProfileReference { get; set; }

        public string? AllowedPorts { get; set; }

        public ICollection<Operation> Operations { get; set; }
            = new List<Operation>();

        [ConcurrencyCheck]
        public Guid ConcurrencyToken { get; set; } = Guid.NewGuid();
    }
}
