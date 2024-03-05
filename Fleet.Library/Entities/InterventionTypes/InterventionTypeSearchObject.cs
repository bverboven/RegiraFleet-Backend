using Regira.Fleet.Abstractions;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }

    public ICollection<int>? OperatorId { get; set; }
}