using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Cars;

public class CarSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Model { get; set; }
    public string? Brand { get; set; }
    public string? CarType { get; set; }
}