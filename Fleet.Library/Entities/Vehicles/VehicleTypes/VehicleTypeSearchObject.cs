using Regira.Fleet.Abstractions;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleTypeSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
}