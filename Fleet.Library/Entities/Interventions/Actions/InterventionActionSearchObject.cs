using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Interventions.Actions;

public class InterventionActionSearchObject : SearchObject
{
    public ICollection<int>? CarId { get; set; }
    public ICollection<int>? OperatorId { get; set; }
    public ICollection<int>? InterventionTypeId { get; set; }
}