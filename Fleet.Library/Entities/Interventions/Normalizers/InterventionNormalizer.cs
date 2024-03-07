using Regira.Fleet.Data;
using Regira.Fleet.Normalizing;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Interventions.Normalizers;

public class InterventionNormalizer(INormalizer normalizer, FleetContext dbContext) : FleetEntityNormalizer<Intervention>(normalizer)
{
    public override void SetNormalizedContent(Intervention item)
    {
        var contentEntries = GetDefaultNormalizedContentEntries(item);

        var vehicle = item.Vehicle ?? dbContext.Vehicles.Find(item.VehicleId);
        contentEntries.Add(vehicle?.NormalizedTitle);
        var supplier = item.Operator ?? dbContext.InterventionOperators.Find(item.OperatorId);
        contentEntries.Add(supplier?.NormalizedTitle);
        var interventionTypeIds = item.InterventionTypes?.Select(x => x.InterventionTypeId);
        if (interventionTypeIds?.Any() == true)
        {
            var interventionTypes = dbContext.InterventionTypes.Where(x => interventionTypeIds.Contains(x.Id));
            contentEntries.AddRange(interventionTypes.Select(x => x.NormalizedTitle));
        }

        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}