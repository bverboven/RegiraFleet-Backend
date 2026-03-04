using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Models.Vehicles;

public class VehicleInterventionType : IEntityWithSerial
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public int InterventionTypeId { get; set; }

    public InterventionType? InterventionType { get; set; }
}