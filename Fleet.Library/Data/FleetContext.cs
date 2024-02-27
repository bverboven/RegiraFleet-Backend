using Microsoft.EntityFrameworkCore;
using Regira.DAL.EFcore.Extensions;
using Regira.Fleet.Bookings;
using Regira.Fleet.Brands;
using Regira.Fleet.Cars;
using Regira.Fleet.CarTypes;
using Regira.Fleet.InterventionTypes;
using Regira.Fleet.Suppliers;
using Regira.Fleet.SupplierTypes;
using Regira.Utilities;

namespace Regira.Fleet.Data;

public class FleetContext : DbContext
{
    public FleetContext(DbContextOptions<FleetContext> options)
        : base(options)
    {
    }


    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarType> CarTypes { get; set; }
    public DbSet<InterventionType> InterventionTypes { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplierType> SupplierTypes { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasIndex(e => e.InvoiceNumber);
            entity.HasIndex(e => e.InvoiceDate);
        });
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasIndex(e => e.Code)
                .IsUnique();
        });
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasIndex(e => e.Code)
                .IsUnique();
            entity.HasIndex(e => e.Name);
        });


        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasIndex(e => e.Code)
                .IsUnique();
            entity.HasIndex(e => e.Title);
        });
        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasIndex(e => e.Code)
                .IsUnique();
            entity.HasIndex(e => e.Title);
        });
        modelBuilder.Entity<InterventionType>(entity =>
        {
            entity.HasIndex(e => e.Code)
                .IsUnique();
            entity.HasIndex(e => e.Title);
        });
        modelBuilder.Entity<SupplierType>(entity =>
        {
            entity.HasIndex(e => e.Code)
                .IsUnique();
            entity.HasIndex(e => e.Title);
        });

        // Decimals
        // https://stackoverflow.com/questions/43277154/entity-framework-core-setting-the-decimal-precision-and-scale-to-all-decimal-p#answer-43282620
        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .ToArray();
        foreach (var property in entityTypes
                     .SelectMany(t => t.GetProperties())
                     .Where(p => TypeUtility.GetSimpleType(p.ClrType) == typeof(decimal)))
        {
            property.SetColumnType("decimal(9, 2)");
        }
    }

    public override int SaveChanges()
    {
        this.AutoTruncateStringsToMaxLengthForEntries();
        return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        this.AutoTruncateStringsToMaxLengthForEntries();
        return base.SaveChangesAsync(cancellationToken);
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.AutoTruncateStringsToMaxLengthForEntries();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        this.AutoTruncateStringsToMaxLengthForEntries();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}