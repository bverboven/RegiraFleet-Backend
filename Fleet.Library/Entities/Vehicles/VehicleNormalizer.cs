using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Vehicles;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleNormalizer(INormalizer normalizer, FleetContextBase dbContext) : FleetEntityNormalizer<Vehicle>(normalizer)
{
    public override void HandleNormalize(Vehicle? item)
    {
        if (item == null)
        {
            return;
        }

        base.HandleNormalize(item);

        // NormalizedTitle & NormalizedContent
        SetNormalizedContent(item!);
    }
    public override void SetNormalizedContent(Vehicle item)
    {
        var brand = item.BrandId.HasValue
            ? item.Brand ?? dbContext.VehicleBrands.Find(item.BrandId)
            : null;
        var type = item.VehicleTypeId.HasValue
            ? item.VehicleType ?? dbContext.VehicleTypes.Find(item.VehicleTypeId)
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
