using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<EnvironmentEntity> Environments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnvironmentEntity>(entity =>
            {
                entity.HasKey(e => e.EnvironmentId);

                entity.Property(e => e.EnvironmentCode)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.EnvironmentName)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Description)
                      .HasMaxLength(255);
            });
        }
    }
}
