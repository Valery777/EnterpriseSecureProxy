using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EnterpriseSecureProxy.src.Domain.Entities;
namespace EnterpriseSecureProxy.src.Infrastructure.Persistence.Configurations
{
    public sealed class ConfigurationHistoryConfiguration
    : IEntityTypeConfiguration<ConfigurationHistory>
    {
        public void Configure(EntityTypeBuilder<ConfigurationHistory> builder)
        {
            builder.ToTable("configuration_history");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EntityType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Action)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ChangeSummary)
                .HasMaxLength(4000);

            builder.Property(x => x.ChangedBy)
                .HasMaxLength(200);

            builder.HasIndex(x => x.EntityType);

            builder.HasIndex(x => x.EntityId);
        }
    }
}
