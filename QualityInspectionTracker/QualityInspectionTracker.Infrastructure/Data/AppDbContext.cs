using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QualityInspectionTracker.Domain;

namespace QualityInspectionTracker.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Inspection> Inspections { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=QITDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.HasIndex(e => e.InspectionDate, "IX_Inspections_InspectionDate");

            entity.HasIndex(e => e.Severity, "IX_Inspections_Severity");

            entity.HasIndex(e => e.Status, "IX_Inspections_Status");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Inspections_CreatedAt");
            entity.Property(e => e.DefectType).HasMaxLength(30);
            entity.Property(e => e.MachineLineId).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.ResolutionNote).HasMaxLength(1000);
            entity.Property(e => e.Severity).HasMaxLength(10);
            entity.Property(e => e.Source)
                .HasMaxLength(20)
                .HasDefaultValue("manual", "DF_Inspections_Source");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Open", "DF_Inspections_Status");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Inspections)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Inspections_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username, "UQ_Users_Username").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Users_CreatedAt");
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Users_IsActive");
            entity.Property(e => e.PasswordHash).HasMaxLength(200);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Supervisor", "DF_Users_Role");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
