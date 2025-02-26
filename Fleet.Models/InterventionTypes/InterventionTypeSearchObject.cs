using Regira.Fleet.Models.Abstractions;

namespace Regira.Fleet.Models.InterventionTypes;

public partial class InterventionTypeSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }

    public ICollection<int>? OperatorId { get; set; }
    public ICollection<int>? VehicleId { get; set; }
}