using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EnterpriseSecureProxy.src.Domain.Entities;
namespace EnterpriseSecureProxy.src.Infrastructure.Persistence.Configurations
{
    public sealed class EncryptionProfileConfiguration
    : IEntityTypeConfiguration<EncryptionProfile>
    {
        public void Configure(EntityTypeBuilder<EncryptionProfile> builder)
        {
            builder.ToTable("encryption_profiles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Algorithm)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.KeyReference)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);
        }
    }
}