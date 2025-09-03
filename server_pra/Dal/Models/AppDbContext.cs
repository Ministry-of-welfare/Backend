using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace server_pra.Dal.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Environment> Environments { get; set; }

    public virtual DbSet<TemplateStatus> TemplateStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\USER\\Ministry-of-welfare.mdf;Integrated Security=True;Connect Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Environment>(entity =>
        {
            entity.HasKey(e => e.EnvironmentId).HasName("PK__Environm__4B909A4913B864C9");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.EnvironmentCode)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.EnvironmentName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TemplateStatus>(entity =>
        {
            entity.HasKey(e => e.TemplateStatusId).HasName("PK__Template__B255A4CFA76FFF8D");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.StatusCode)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
