using Microsoft.EntityFrameworkCore;
using Regira.Entities.Attachments.Models;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.Interventions.Actions;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Data;

public interface IFleetDbContext
{
    DbSet<Attachment<int>> Attachments { get; set; }
    DbSet<Intervention> Interventions { get; set; }
    DbSet<InterventionAction> InterventionActions { get; set; }
    DbSet<InterventionLabel> InterventionLabels { get; set; }
    DbSet<InterventionAttachment> InterventionAttachments { get; set; }
    DbSet<InterventionType> InterventionTypes { get; set; }
    DbSet<Operator> InterventionOperators { get; set; }
    DbSet<OperatorLabel> InterventionOperatorLabels { get; set; }
    DbSet<OperatorAttachment> InterventionOperatorAttachments { get; set; }
    DbSet<Vehicle> Vehicles { get; set; }
    DbSet<Brand> VehicleBrands { get; set; }
    DbSet<VehicleType> VehicleTypes { get; set; }
    DbSet<VehicleLabel> VehicleLabels { get; set; }
    DbSet<VehicleAttachment> VehicleAttachments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
