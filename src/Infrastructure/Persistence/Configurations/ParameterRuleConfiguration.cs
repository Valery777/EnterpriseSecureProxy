using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EnterpriseSecureProxy.src.Domain.Entities;
namespace EnterpriseSecureProxy.src.Infrastructure.Persistence.Configurations
{
    public sealed class ParameterRuleConfiguration
     : IEntityTypeConfiguration<ParameterRule>
    {
        public void Configure(EntityTypeBuilder<ParameterRule> builder)
        {
            builder.ToTable("parameter_rules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.DataType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Location)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.RegexPattern)
                .HasMaxLength(500);

            builder.HasOne(x => x.Operation)
                .WithMany(x => x.ParameterRules)
                .HasForeignKey(x => x.OperationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x =>
                new { x.OperationId, x.Name });
        }
    }
}