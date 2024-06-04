using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleNormalizer(INormalizer normalizer, FleetContextBase dbContext) : FleetEntityNormalizer<Vehicle>(normalizer)
{
    private List<Brand> _brands = null!;
    private List<VehicleType> _vehicleTypes = null!;

    public override async Task HandleNormalizeMany(IEnumerable<Vehicle?> items, bool recursive = false)
    {
        var brandIds = items.Select(x => x?.BrandId).Where(id => id.HasValue).Distinct().ToArray();
        _brands = await dbContext.VehicleBrands.Where(x => brandIds.Contains(x.Id)).ToListAsync();
        var typeIds = items.Select(x => x?.VehicleTypeId).Where(id => id.HasValue).Distinct().ToArray();
        _vehicleTypes = await dbContext.VehicleTypes.Where(x => typeIds.Contains(x.Id)).ToListAsync();

        await base.HandleNormalizeMany(items, recursive);
    }
    public override void HandleNormalize(Vehicle? item, bool recursive = false)
    {
        if (item == null)
        {
            return;
        }

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
            type?.Code,
            type?.NormalizedTitle,
            brand?.Code,
            brand?.NormalizedTitle,
            item.Model
        };
        item.NormalizedTitle = string.Join(' ', titleEntries.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());

        var contentEntries = new List<string?>
        {
            item.Code!,
            type?.NormalizedTitle,
            type?.Code,
            brand?.NormalizedTitle,
            brand?.Code,
            item.IdentificationNumber,
            item.Model,
            item.Description
        };
        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
    }
}
