using EnterpriseSecureProxy.src.Domain.Common;

namespace EnterpriseSecureProxy.src.Domain.Entities
{
    public sealed class HeaderRule : BaseEntity
    {
        public Guid OperationId { get; set; }

        public string HeaderName { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public Operation Operation { get; set; } = null!;
    }
}
