using Regira.Fleet.Models.Abstractions;

namespace Regira.Fleet.Models.Vehicles;

public record VehicleSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Model { get; set; }
    public ICollection<int>? BrandId { get; set; }
    public ICollection<int>? VehicleTypeId { get; set; }
    public string? Brand { get; set; }
    public string? VehicleType { get; set; }

    public string? Title { get; set; }

    public bool? HasIntervention { get; set; }
}