using Microsoft.EntityFrameworkCore;
using Cwiczenia4Api.Models;

namespace Cwiczenia4Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Pc> Pcs => Set<Pc>();
    public DbSet<Component> Components => Set<Component>();
    public DbSet<PcComponent> PcComponents => Set<PcComponent>();
    public DbSet<ComponentManufacturer> ComponentManufacturers => Set<ComponentManufacturer>();
    public DbSet<ComponentType> ComponentTypes => Set<ComponentType>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pc>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Weight).HasColumnType("float");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Description).HasMaxLength(300);

            entity.HasOne(e => e.Manufacturer)
                .WithMany(m => m.Components)
                .HasForeignKey(e => e.ComponentManufacturersId);

            entity.HasOne(e => e.Type)
                .WithMany(t => t.Components)
                .HasForeignKey(e => e.ComponentTypesId);
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).IsRequired().HasMaxLength(30);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(300);
            entity.Property(e => e.FoundationDate).HasColumnType("date");
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<PcComponent>(entity =>
        {
            entity.HasKey(e => new { e.PCId, e.ComponentCode });

            entity.HasOne(e => e.Pc)
                .WithMany(p => p.PcComponents)
                .HasForeignKey(e => e.PCId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Component)
                .WithMany(c => c.PcComponents)
                .HasForeignKey(e => e.ComponentCode);
        });

        // seed data
    }
}