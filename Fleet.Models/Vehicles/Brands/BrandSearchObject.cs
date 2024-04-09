using Regira.Fleet.Models.Abstractions;

namespace Regira.Fleet.Models.Vehicles.Brands;

public class BrandSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }

}