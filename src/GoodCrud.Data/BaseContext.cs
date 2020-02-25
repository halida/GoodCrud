using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using GoodCrud.Domain;

namespace GoodCrud.Data
{
    public abstract class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AssignTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AssignTimestamps();
            return base.SaveChangesAsync();
        }

        protected void AssignTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;
                entity.UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added && entity.CreatedAt == null)
                {
                    entity.CreatedAt = entity.UpdatedAt;
                }
            }

        }
    }
}
