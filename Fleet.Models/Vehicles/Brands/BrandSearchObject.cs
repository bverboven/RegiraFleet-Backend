using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Models.Abstractions;

namespace Regira.Fleet.Models.Vehicles.Brands;

public class BrandSearchObject : FleetSearchObject, IHasCode, IHasTitle
{
    public string? Code { get; set; }
    public string? Title { get; set; }
}