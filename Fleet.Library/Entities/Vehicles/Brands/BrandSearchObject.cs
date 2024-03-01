using Regira.Fleet.Abstractions;

namespace Regira.Fleet.Entities.Vehicles.Brands;

public class BrandSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }

}