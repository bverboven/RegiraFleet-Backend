using Microsoft.EntityFrameworkCore;
using Regira.DAL.EFcore.Extensions;
using Regira.Entities.Attachments.Models;
using Regira.Fleet.Entities.InterventionOperators.Addresses;
using Regira.Fleet.Entities.InterventionOperators.ContactData;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.Action;
using Regira.Fleet.Entities.Interventions.Invoices;
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
    public DbSet<Intervention> Interventions { get; set; } = null!;
    public DbSet<InterventionAction> InterventionActions { get; set; } = null!;
    public DbSet<InterventionAttachment> InterventionAttachments { get; set; } = null!;
    public DbSet<InterventionType> InterventionTypes { get; set; } = null!;
    public DbSet<Operator> InterventionOperators { get; set; } = null!;
    public DbSet<OperatorAttachment> InterventionOperatorAttachments { get; set; } = null!;
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

        // Interventions
        modelBuilder.Entity<InterventionType>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => new { e.ClientId, e.Title })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });
        modelBuilder.Entity<InterventionTypeTranslation>(entity =>
        {
            entity.ToTable("intervention_type_translations");
            entity.HasKey(e => new { e.InterventionTypeId, e.LangCode });
        });
        modelBuilder.Entity<Intervention>(entity =>
        {
            entity.HasIndex(e => e.ClientId);

            // Attachments
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.ObjectId)
                .HasPrincipalKey(e => e.Id);
        });
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("intervention_invoices")
                .HasIndex(e => e.InvoiceNumber);
        });
        modelBuilder.Entity<InterventionAction>(entity =>
        {
        });
        modelBuilder.Entity<OperatorAddress>(entity =>
        {
            entity
                .ToTable("intervention_operator_addresses")
                .HasIndex(cd => cd.NormalizedContent);
        });
        modelBuilder.Entity<OperatorContactData>(entity =>
        {
            entity
                .ToTable("intervention_operator_contactdata")
                .HasIndex(cd => cd.NormalizedValue);
        });
        modelBuilder.Entity<Operator>(entity =>
        {
            //entity.ToTable("intervention_operators");
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);

            // Addresses
            entity
                .HasMany(e => e.Addresses)
                .WithOne()
                .HasPrincipalKey(e => e.Id);

            // ContactData
            entity
                .HasMany(e => e.ContactData)
                .WithOne()
                .HasPrincipalKey(e => e.Id);

            // Attachments
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .HasForeignKey(e => e.ObjectId)
                .HasPrincipalKey(e => e.Id);
        });
        modelBuilder.Entity<OperatorInterventionType>(entity =>
        {
            entity.ToTable("intervention_operator_intervention_types");
        });

        // Vehicles
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("vehicle_brands");
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => new { e.ClientId, e.Title })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });
        modelBuilder.Entity<BrandTranslation>(entity =>
        {
            entity.ToTable("vehicle_brand_translations");
            entity.HasKey(e => new { e.BrandId, e.LangCode });
        });
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(cd => cd.NormalizedTitle);

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
        modelBuilder.Entity<VehicleTypeTranslation>(entity =>
        {
            entity.ToTable("vehicle_type_translations");
            entity.HasKey(e => new { e.VehicleTypeId, e.LangCode });
        });
        modelBuilder.Entity<VehicleInterventionType>(entity =>
        {
            entity.ToTable("vehicle_intervention_types");
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