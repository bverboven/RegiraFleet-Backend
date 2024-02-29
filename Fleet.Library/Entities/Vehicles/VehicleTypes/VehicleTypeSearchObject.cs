using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleTypeSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
}