using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Infrastructure.Persistence.Configurations
{

    public sealed class OperationConfiguration
        : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.ToTable("operations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OperationKey)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(x => x.OperationKey)
                .IsUnique();

            builder.Property(x => x.HttpMethod)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.RelativePath)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.MaxRequestSizeBytes)
                .IsRequired();

            builder.Property(x => x.MaxResponseSizeBytes)
                .IsRequired();

            builder.Property(x => x.RequiresEncryption)
                .IsRequired();

            builder.Property(x => x.AllowUnknownParameters)
                .IsRequired();

            builder.HasOne(x => x.Destination)
                .WithMany(x => x.Operations)
                .HasForeignKey(x => x.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.EncryptionProfile)
                .WithMany()
                .HasForeignKey(x => x.EncryptionProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // PostgreSQL optimistic concurrency
            builder.Property(x => x.xmin).IsRowVersion();
       
            builder.HasIndex(x => x.IsActive);
        }
    }
}
