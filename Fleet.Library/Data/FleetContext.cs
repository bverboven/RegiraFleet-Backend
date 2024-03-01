using Microsoft.EntityFrameworkCore;
using Regira.DAL.EFcore.Extensions;
using Regira.Entities.Attachments.Models;
using Regira.Fleet.Entities.Clients;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions.Actions;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;

namespace Regira.Fleet.Data;

public class FleetContext(DbContextOptions<FleetContext> options) : DbContext(options)
{
    // Attachments
    public DbSet<Attachment<int>> Attachments { get; set; } = null!;
    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<InterventionAction> InterventionActions { get; set; } = null!;
    public DbSet<InterventionActionAttachment> InterventionActionAttachments { get; set; } = null!;
    public DbSet<InterventionType> InterventionTypes { get; set; } = null!;
    public DbSet<InterventionOperator> InterventionOperators { get; set; } = null!;
    public DbSet<InterventionOperatorAttachment> InterventionOperatorAttachments { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<VehicleAttachment> VehicleAttachments { get; set; } = null!;
    public DbSet<VehicleType> VehicleTypes { get; set; } = null!;


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
            entity.HasIndex(e => new { e.ClientId, e.Title })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });
        modelBuilder.Entity<InterventionAction>(entity =>
        {
            entity.HasIndex(e => e.ClientId);

            // Invoice
            entity.OwnsMany(e => e.Invoices, e =>
            {
                e.ToTable("invoices");
                e.HasIndex(i => i.InvoiceNumber);
                e.HasIndex(i => i.InvoiceDate);
            });

            // Intervention Types
            entity.HasMany(e => e.InterventionTypes)
                .WithMany()
                .UsingEntity("intervention_intervention_types");

            // Attachments
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.ObjectId)
                .HasPrincipalKey(e => e.Id);
        });
        modelBuilder.Entity<InterventionOperator>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
            // Intervention Types
            entity.HasMany(e => e.InterventionTypes)
                .WithMany()
                .UsingEntity("intervention_operator_intervention_types");
            // Addresses
            entity.OwnsMany(e => e.Addresses, e =>
            {
                e.ToTable("intervention_operator_addresses");
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
        modelBuilder.Entity<InterventionType>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => new { e.ClientId, e.Title })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => new { e.ClientId, e.BrandId, e.Model })
                .IsUnique();

            // Attachments
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.ObjectId)
                .HasPrincipalKey(e => e.Id);
        });
        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => new { e.ClientId, e.Title })
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