using Regira.Fleet.Data;
using Regira.Fleet.Normalizing.Abstractions;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleNormalizer(INormalizer normalizer, FleetContext dbContext) : FleetEntityNormalizerBase<Vehicle>(normalizer)
{
    public override void HandleNormalize(Vehicle? item)
    {
        if (item == null)
        {
            return;
        }

        // NormalizedTitle & NormalizedContent
        SetNormalizedContent(item!);
    }
    public override void SetNormalizedContent(Vehicle item)
    {
        var titleEntries = new List<string?> { item.Code! };
        var contentEntries = GetDefaultNormalizedContentEntries(item);
        contentEntries.Add(item.Code);
        contentEntries.Add(item.Model);

        if (item.BrandId.HasValue)
        {
            var brand = item.Brand ?? dbContext.Brands.Find(item.BrandId);
            contentEntries.Add(brand?.NormalizedTitle);
            titleEntries.Add(brand?.NormalizedTitle);
        }
        if (item.VehicleTypeId.HasValue)
        {
            var type = item.VehicleType ?? dbContext.VehicleTypes.Find(item.VehicleTypeId);
            contentEntries.Add(type?.NormalizedTitle);
        }

        titleEntries.Add(item.Model);

        item.NormalizedTitle = string.Join(' ', titleEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}
