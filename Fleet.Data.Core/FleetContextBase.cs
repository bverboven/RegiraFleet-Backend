using Microsoft.EntityFrameworkCore;
using Regira.DAL.EFcore.Extensions;
using Regira.Entities.Attachments.Models;
using Regira.Fleet.Models.InterventionOperators.Addresses;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.Interventions.Action;
using Regira.Fleet.Models.Interventions.Invoices;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Data;

public abstract class FleetContextBase(DbContextOptions options) : DbContext(options), IFleetDbContext
{
    public DbSet<Attachment<int>> Attachments { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<InterventionAction> InterventionActions { get; set; }
    public DbSet<InterventionAttachment> InterventionAttachments { get; set; }
    public DbSet<OperatorAttachment> InterventionOperatorAttachments { get; set; }
    public DbSet<Operator> InterventionOperators { get; set; }
    public DbSet<Intervention> Interventions { get; set; }
    public DbSet<InterventionType> InterventionTypes { get; set; }
    public DbSet<VehicleAttachment> VehicleAttachments { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }


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
            entity.HasIndex(e => e.InvoiceNumber);
        });
        modelBuilder.Entity<InterventionAction>(entity =>
        {
        });
        modelBuilder.Entity<OperatorAddress>(entity =>
        {
            // causes error in MySQL
            //entity.HasIndex(cd => cd.NormalizedContent);
        });
        modelBuilder.Entity<OperatorContactData>(entity =>
        {
            entity.HasIndex(cd => cd.NormalizedValue);
        });
        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity
                .HasIndex(e => new { e.ClientId, e.Code })
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

        // Vehicles
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.ClientId, e.Code })
                .IsUnique();
            entity.HasIndex(e => new { e.ClientId, e.Title })
                .IsUnique();
            entity.HasIndex(e => e.NormalizedTitle);
        });
        modelBuilder.Entity<BrandTranslation>(entity =>
        {
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
            entity.HasKey(e => new { e.VehicleTypeId, e.LangCode });
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


    public virtual bool ILike(string matchExpression, string pattern)
        => EF.Functions.Like(matchExpression, pattern);
}