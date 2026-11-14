using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EnterpriseSecureProxy.src.Domain.Entities;

namespace EnterpriseSecureProxy.src.Infrastructure.Persistence.Configurations
{
    public sealed class DestinationConfiguration
     : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> builder)
        {
            builder.ToTable("destinations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.BaseUrl)
                .HasMaxLength(2048)
                .IsRequired();

            builder.Property(x => x.TimeoutSeconds)
                .IsRequired();

            builder.Property(x => x.AuthProfileReference)
                .HasMaxLength(500);

            builder.Property(x => x.AllowedPorts)
                .HasMaxLength(200);

            builder.Property(x => x.CreatedUtc)
                .IsRequired();

            builder.Property(x => x.UpdatedUtc)
                .IsRequired();

            builder.HasIndex(x => x.Name);

            builder.HasIndex(x => x.IsActive);
        }
    }
}
