using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Models.InterventionOperators.Addresses;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions.Invoices;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Data.PostgreSQL;

public class FleetPostgresContext(DbContextOptions<FleetPostgresContext> options) : FleetContextBase(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Interventions
        modelBuilder.Entity<InterventionTypeTranslation>(entity =>
        {
            entity.ToTable("intervention_type_translations");
        });
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("intervention_invoices");
        });
        modelBuilder.Entity<OperatorAddress>(entity =>
        {
            entity.ToTable("intervention_operator_addresses");
            // causes error in MySQL
            entity.HasIndex(cd => cd.NormalizedContent);
        });
        modelBuilder.Entity<OperatorContactData>(entity =>
        {
            entity.ToTable("intervention_operator_contactdata");
        });
        modelBuilder.Entity<OperatorInterventionType>(entity =>
        {
            entity.ToTable("intervention_operator_intervention_types");
        });

        // Vehicles
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("vehicle_brands");
        });
        modelBuilder.Entity<VehicleTypeTranslation>(entity =>
        {
            entity.ToTable("vehicle_type_translations");
        });
        modelBuilder.Entity<VehicleInterventionType>(entity =>
        {
            entity.ToTable("vehicle_intervention_types");
        });
    }
}