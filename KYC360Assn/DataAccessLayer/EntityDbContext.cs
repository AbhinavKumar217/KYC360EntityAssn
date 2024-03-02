using KYC360Assn.Models.DBEntities;
using Microsoft.EntityFrameworkCore;

namespace KYC360Assn.DataAccessLayer
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Name> Names { get; set; }

        public DbSet<Date> Dates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>()
                .HasMany(e => e.Addresses)
                .WithOne(a => a.Entity)
                .HasForeignKey(a => a.EntityId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entity>()
                .HasMany(e => e.Dates)
                .WithOne(d => d.Entity)
                .HasForeignKey(d => d.EntityId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entity>()
                .HasMany(e => e.Names)
                .WithOne(n => n.Entity)
                .HasForeignKey(n => n.EntityId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }


    }
}
