using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Infrastructure.Persistence.Configurations
{
    public sealed class RequestLogConfiguration
    : IEntityTypeConfiguration<RequestLog>
    {
        public void Configure(EntityTypeBuilder<RequestLog> builder)
        {
            builder.ToTable("request_logs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CorrelationId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.OperationKey)
                .HasMaxLength(200);

            builder.Property(x => x.RejectionReason)
                .HasMaxLength(2000);

            builder.Property(x => x.Outcome)
                .HasConversion<int>();

            builder.HasIndex(x => x.CorrelationId);

            builder.HasIndex(x => x.StartedUtc);

            builder.HasIndex(x =>
                new { x.OperationKey, x.StartedUtc });
        }
    }
}
