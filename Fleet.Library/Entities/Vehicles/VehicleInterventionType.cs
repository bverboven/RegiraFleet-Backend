using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Entities.InterventionTypes;

namespace Regira.Fleet.Entities.Vehicles;

[PrimaryKey(nameof(VehicleId), nameof(InterventionTypeId))]
public class VehicleInterventionType
{
    public int VehicleId { get; set; }
    public int InterventionTypeId { get; set; }

    public Vehicle? Vehicle { get; set; }
    public InterventionType? InterventionType { get; set; }
}
