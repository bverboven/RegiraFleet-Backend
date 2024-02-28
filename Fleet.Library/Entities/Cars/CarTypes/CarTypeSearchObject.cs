using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Cars.CarTypes;

public class CarTypeSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
}