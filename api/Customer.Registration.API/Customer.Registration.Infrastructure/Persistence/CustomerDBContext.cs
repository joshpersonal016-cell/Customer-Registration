using Microsoft.EntityFrameworkCore;
using Customer.Registration.Domain.Entities;
using Customer.Registration.Domain.Entities.Common;

namespace Customer.Registration.Infrastructure.Persistence
{
    public class CustomerDBContext(DbContextOptions<CustomerDBContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.CustomerEntitiy> Customers => Set<CustomerEntitiy>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Scan configuration from the assembly containing the context and apply them
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDBContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Set CreatedAt for new entities
            var entries = ChangeTracker.Entries<BaseEntity>();
            var now = DateTime.UtcNow;
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }   
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
