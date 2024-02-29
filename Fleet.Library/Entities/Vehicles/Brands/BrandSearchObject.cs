using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Vehicles.Brands;

public class BrandSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }

}