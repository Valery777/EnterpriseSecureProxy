using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EnterpriseSecureProxy.src.Domain.Entities;
namespace EnterpriseSecureProxy.src.Infrastructure.Persistence.Configurations
{
    public sealed class HeaderRuleConfiguration
    : IEntityTypeConfiguration<HeaderRule>
    {
        public void Configure(EntityTypeBuilder<HeaderRule> builder)
        {
            builder.ToTable("header_rules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HeaderName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.IsRequired)
                .IsRequired();

            builder.HasOne(x => x.Operation)
                .WithMany(x => x.HeaderRules)
                .HasForeignKey(x => x.OperationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x =>
                new { x.OperationId, x.HeaderName });
        }
    }
}