using Microsoft.EntityFrameworkCore;
using portal.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace portal.Domain
{
    public class PortalDbContext : DbContext
    {
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<Subscription> Subcriptions { get; set; }

        public PortalDbContext(DbContextOptions<PortalDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Journal>()
                .Property(c => c.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder.Entity<Edition>()
               .Property(c => c.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder.Entity<Subscription>()
               .Property(c => c.Id)
               .HasDefaultValueSql("NEWSEQUENTIALID()");

            base.OnModelCreating(modelBuilder);
        }
    }
}