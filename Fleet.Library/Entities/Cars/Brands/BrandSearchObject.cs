using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Cars.Brands;

public class BrandSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }

}