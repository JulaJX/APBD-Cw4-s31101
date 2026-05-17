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

        modelBuilder.Entity<ComponentManufacturer>().HasData(
        new ComponentManufacturer
        {
            Id = 1,
            Abbreviation = "ASUS",
            FullName = "ASUSTeK Computer Inc.",
            FoundationDate = new DateTime(1989, 4, 1)
        },
        new ComponentManufacturer
        {
            Id = 2,
            Abbreviation = "MSI",
            FullName = "Micro-Star International",
            FoundationDate = new DateTime(1986, 8, 1)
        },
        new ComponentManufacturer
        {
            Id = 3,
            Abbreviation = "GIG",
            FullName = "Gigabyte Technology",
            FoundationDate = new DateTime(1986, 1, 1)
        }
    );

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<Pc>().HasData(
            new Pc
            {
                Id = 1,
                Name = "Gaming Beast X",
                Weight = 12.5,
                Warranty = 36,
                CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0),
                Stock = 5
            },
            new Pc
            {
                Id = 2,
                Name = "Office Mini Pro",
                Weight = 4.2,
                Warranty = 24,
                CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0),
                Stock = 12
            },
            new Pc
            {
                Id = 3,
                Name = "Budget Starter",
                Weight = 6.0,
                Warranty = 12,
                CreatedAt = new DateTime(2026, 3, 1, 10, 0, 0),
                Stock = 20
            }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component
            {
                Code = "CPU-0001",
                Name = "Ryzen 7 7800X3D",
                Description = "High-end gaming CPU",
                ComponentManufacturersId = 1,
                ComponentTypesId = 1
            },
            new Component
            {
                Code = "GPU-0001",
                Name = "RTX 4070",
                Description = "Gaming GPU",
                ComponentManufacturersId = 2,
                ComponentTypesId = 2
            },
            new Component
            {
                Code = "RAM-0001",
                Name = "16GB DDR5",
                Description = "Memory kit",
                ComponentManufacturersId = 3,
                ComponentTypesId = 3
            }
        );

        modelBuilder.Entity<PcComponent>().HasData(
            new PcComponent { PCId = 1, ComponentCode = "CPU-0001", Amount = 1 },
            new PcComponent { PCId = 1, ComponentCode = "GPU-0001", Amount = 1 },
            new PcComponent { PCId = 1, ComponentCode = "RAM-0001", Amount = 2 },
            new PcComponent { PCId = 2, ComponentCode = "CPU-0001", Amount = 1 },
            new PcComponent { PCId = 2, ComponentCode = "RAM-0001", Amount = 1 }
        );
        }
}