using EnterpriseSecureProxy.src.Domain.Common;

namespace EnterpriseSecureProxy.src.Domain.Entities
{
    public sealed class EncryptionProfile : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Algorithm { get; set; } = "AES-GCM";

        public string KeyReference { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
