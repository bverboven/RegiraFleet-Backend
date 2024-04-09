using Regira.Fleet.Models.Abstractions;

namespace Regira.Fleet.Models.Interventions;

public class InterventionSearchObject : FleetSearchObject
{
    public ICollection<int>? VehicleId { get; set; }
    public ICollection<int>? OperatorId { get; set; }
    public ICollection<int>? InterventionTypeId { get; set; }

    public ICollection<int>? VehicleTypeId { get; set; }
    public ICollection<int>? BrandId { get; set; }

    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}