using EnterpriseSecureProxy.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EnterpriseSecureProxy.src.Infrastructure
{
    public sealed class ProxyDbContext : DbContext
    {
        public ProxyDbContext(DbContextOptions<ProxyDbContext> options)
            : base(options)
        {
        }
        public DbSet<Destination> Destinations => Set<Destination>();
        public DbSet<Operation> Operations => Set<Operation>();
        public DbSet<ParameterRule> ParameterRules => Set<ParameterRule>();
        public DbSet<HeaderRule> HeaderRules => Set<HeaderRule>();
        public DbSet<EncryptionProfile> EncryptionProfiles => Set<EncryptionProfile>();
        public DbSet<RequestLog> RequestLogs => Set<RequestLog>();
        public DbSet<ConfigurationHistory> ConfigurationHistory => Set<ConfigurationHistory>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ProxyDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
