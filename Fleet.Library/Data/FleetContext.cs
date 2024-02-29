using Microsoft.EntityFrameworkCore;
using Regira.DAL.EFcore.Extensions;
using Regira.Entities.Attachments.Models;
using Regira.Fleet.Entities.Cars;
using Regira.Fleet.Entities.Cars.Brands;
using Regira.Fleet.Entities.Cars.CarTypes;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Suppliers;
using Regira.Fleet.Entities.Suppliers.SupplierTypes;

namespace Regira.Fleet.Data;

public class FleetContext(DbContextOptions<FleetContext> options) : DbContext(options)
{
    // Attachments
    public DbSet<Attachment<int>> Attachments { get; set; } = null!;
    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<CarAttachment> CarAttachments { get; set; } = null!;
    public DbSet<CarType> CarTypes { get; set; } = null!;
    public DbSet<Intervention> Interventions { get; set; } = null!;
    public DbSet<InterventionAttachment> InterventionAttachments { get; set; } = null!;
    public DbSet<InterventionType> InterventionTypes { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<SupplierAttachment> SupplierAttachments { get; set; } = null!;
    public DbSet<SupplierType> SupplierTypes { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();

            // Attachments
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.ObjectId)
                .HasPrincipalKey(e => e.Id);
        });
        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });
        modelBuilder.Entity<Intervention>(entity =>
        {
            entity.HasIndex(e => e.ClientId);

            // Invoice
            entity.OwnsOne(e => e.Invoice, e =>
            {
                e.ToTable("invoices");
                e.HasIndex(i => i.InvoiceNumber);
                e.HasIndex(i => i.InvoiceDate);
            });

            // Attachments
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.ObjectId)
                .HasPrincipalKey(e => e.Id);
        });
        modelBuilder.Entity<InterventionType>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);

            // Addresses
            entity.OwnsMany(e => e.Addresses, e =>
            {
                e.ToTable("supplier_addresses");
                e.HasIndex(cd => cd.NormalizedContent);
            });

            // ContactData
            entity.OwnsMany(e => e.ContactData, e =>
            {
                e.HasIndex(cd => cd.DataType);
                e.HasIndex(cd => cd.NormalizedValue);
            });

            // Attachments
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.ObjectId)
                .HasPrincipalKey(e => e.Id);
        });
        modelBuilder.Entity<SupplierType>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });

        // Decimals
        modelBuilder.SetDecimalPrecisionConvention(9, 2);
    }


    // AutoTruncate
    public override int SaveChanges()
    {
        this.AutoTruncateStringsToMaxLengthForEntries();
        return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        this.AutoTruncateStringsToMaxLengthForEntries();
        return base.SaveChangesAsync(cancellationToken);
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.AutoTruncateStringsToMaxLengthForEntries();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
    {
        this.AutoTruncateStringsToMaxLengthForEntries();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}