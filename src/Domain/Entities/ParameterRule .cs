using EnterpriseSecureProxy.src.Domain.Common;
using EnterpriseSecureProxy.src.Domain.Enums;

namespace EnterpriseSecureProxy.src.Domain.Entities
{
    public sealed class ParameterRule : BaseEntity
    {
        public Guid OperationId { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public ParameterDataType DataType { get; set; }

        public ParameterLocation Location { get; set; }

        public int? MaxLength { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public string? RegexPattern { get; set; }

        public Operation Operation { get; set; } = null!;
    }
}
