using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionSearchObject : SearchObject
{
    public ICollection<int>? CarId { get; set; }
    public ICollection<int>? SupplierId { get; set; }
    public ICollection<int>? InterventionTypeId { get; set; }
}