using Regira.Fleet.Abstractions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionSearchObject : FleetSearchObject
{
    public ICollection<int>? VehicleId { get; set; }
    public ICollection<int>? OperatorId { get; set; }
    public ICollection<int>? InterventionTypeId { get; set; }
}