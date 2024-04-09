using Regira.Fleet.Models.Abstractions;

namespace Regira.Fleet.Models.Vehicles.VehicleTypes;

public class VehicleTypeSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
}