using Regira.Fleet.Abstractions;

namespace Regira.Fleet.Entities.Interventions.Actions;

public class InterventionActionSearchObject : FleetSearchObject
{
    public ICollection<int>? CarId { get; set; }
    public ICollection<int>? OperatorId { get; set; }
    public ICollection<int>? InterventionTypeId { get; set; }
}