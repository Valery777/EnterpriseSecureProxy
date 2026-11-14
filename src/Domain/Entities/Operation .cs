using EnterpriseSecureProxy.src.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseSecureProxy.src.Domain.Entities
{
    public sealed class Operation : BaseEntity
    {
        public string OperationKey { get; set; } = string.Empty;

        public Guid DestinationId { get; set; }

        public string HttpMethod { get; set; } = string.Empty;

        public string RelativePath { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public long MaxRequestSizeBytes { get; set; }

        public long MaxResponseSizeBytes { get; set; }

        public bool RequiresEncryption { get; set; }

        public bool AllowUnknownParameters { get; set; }

        public Guid? EncryptionProfileId { get; set; }

        //SQL Server
        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        // PostgreSQL concurrency token
        public uint xmin { get; private set; }

        public Destination Destination { get; set; } = null!;

        public EncryptionProfile? EncryptionProfile { get; set; }

        public ICollection<ParameterRule> ParameterRules { get; set; }
            = new List<ParameterRule>();

        public ICollection<HeaderRule> HeaderRules { get; set; }
            = new List<HeaderRule>();

        [ConcurrencyCheck]
        public Guid ConcurrencyToken { get; set; } = Guid.NewGuid();
    }
}
