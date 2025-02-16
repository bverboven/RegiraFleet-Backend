using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Vehicles.Normalizers;

public class VehicleNormalizer(INormalizer normalizer, FleetContextBase dbContext) : FleetEntityNormalizer<Vehicle>(normalizer)
{
    public override bool IsExclusive => true;

    private List<Brand> _brands = null!;
    private List<VehicleType> _vehicleTypes = null!;

    public override async Task HandleNormalizeMany(IEnumerable<Vehicle> items)
    {
        var itemList = items as Vehicle[] ?? items.ToArray();
        var brandIds = itemList.Select(x => x.BrandId).Where(id => id.HasValue).Distinct().ToArray();
        _brands = await dbContext.VehicleBrands.Where(x => brandIds.Contains(x.Id)).ToListAsync();
        var typeIds = itemList.Select(x => x.VehicleTypeId).Where(id => id.HasValue).Distinct().ToArray();
        _vehicleTypes = await dbContext.VehicleTypes.Where(x => typeIds.Contains(x.Id)).ToListAsync();

        await base.HandleNormalizeMany(itemList);
    }
    public override Task HandleNormalize(Vehicle item)
    {
        base.HandleNormalize(item);

        var brand = item.BrandId.HasValue
            ? item.Brand ?? _brands.Find(x => x.Id == item.BrandId)
            : null;
        var type = item.VehicleTypeId.HasValue
            ? item.VehicleType ?? _vehicleTypes.Find(x => x.Id == item.VehicleTypeId)
            : null;

        var titleEntries = new List<string?>
        {
            item.Code!,
            type?.NormalizedTitle,
            brand?.NormalizedTitle,
            DefaultPropertyNormalizer.Normalize(item.Model)
        }.Where(x => !string.IsNullOrWhiteSpace(x)).SelectMany(x => x!.Split(' ')).Distinct();
        item.NormalizedTitle = string.Join(' ', titleEntries);

        var contentEntries = new List<string?>
        {
            item.Code!,
            type?.NormalizedTitle,
            brand?.NormalizedTitle,
            item.IdentificationNumber,
            DefaultPropertyNormalizer.Normalize(item.Model),
            item.Description
        }.Where(x => !string.IsNullOrWhiteSpace(x)).SelectMany(x => x!.Split(' ')).Distinct();
        item.NormalizedContent = string.Join(' ', contentEntries);

        return Task.CompletedTask;
    }
}
