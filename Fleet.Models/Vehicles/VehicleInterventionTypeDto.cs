using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Models.Vehicles;

public class VehicleInterventionTypeDto
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public int InterventionTypeId { get; set; }

    public InterventionTypeDto? InterventionType { get; set; }
}