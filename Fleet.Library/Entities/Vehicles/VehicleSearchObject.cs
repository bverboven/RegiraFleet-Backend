using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Model { get; set; }
    public string? Brand { get; set; }
    public string? VehicleType { get; set; }
}