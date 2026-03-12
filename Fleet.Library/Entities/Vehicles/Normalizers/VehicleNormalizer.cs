using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Normalizing.Abstractions;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Vehicles.Normalizers;

public class VehicleNormalizer(INormalizer normalizer, IEntityNormalizer<IEntityLabel> labelNormalizer, FleetContextBase dbContext) : FleetEntityNormalizer<Vehicle>(normalizer)
{
    public override bool IsExclusive => true;

    private List<Brand> _brands = null!;
    private List<VehicleType> _vehicleTypes = null!;

    public override async Task HandleNormalizeMany(IEnumerable<Vehicle> items)
    {
        var itemList = items as Vehicle[] ?? items.ToArray();
        var brandIds = itemList.Select(x => x.BrandId ?? 0).Where(id => id > 0).Distinct().ToList();
        _brands = await dbContext.VehicleBrands.Where(x => brandIds.Contains(x.Id)).ToListAsync();
        var typeIds = itemList.Select(x => x.VehicleTypeId ?? 0).Where(id => id > 0).Distinct().ToList();
        _vehicleTypes = await dbContext.VehicleTypes.Where(x => typeIds.Contains(x.Id)).ToListAsync();

        await base.HandleNormalizeMany(itemList);
    }
    public override async Task HandleNormalize(Vehicle item)
    {
        var brand = item.BrandId.HasValue
            ? item.Brand ?? _brands.Find(x => x.Id == item.BrandId)
            : null;
        var type = item.VehicleTypeId.HasValue
            ? item.VehicleType ?? _vehicleTypes.Find(x => x.Id == item.VehicleTypeId)
            : null;

        var titleEntries = new List<string?>
            {
                item.Code!,
                item.IdentificationNumber,
                type?.NormalizedTitle,
                brand?.NormalizedTitle,
                DefaultPropertyNormalizer.Normalize(item.Model)
            }
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .SelectMany(x => x!.Split(' '))
            .Distinct();
        item.NormalizedTitle = string.Join(' ', titleEntries);

        var contentEntries = GetDefaultNormalizedContentEntries(item);

        // Labels
        if (item.Labels?.Any() == true)
        {
            await labelNormalizer.HandleNormalizeMany(item.Labels);
            contentEntries.AddRange(item.Labels.Select(a => a.NormalizedContent));
        }

        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}
