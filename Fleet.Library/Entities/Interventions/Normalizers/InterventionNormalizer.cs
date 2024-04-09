using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Interventions;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Interventions.Normalizers;

public class InterventionNormalizer(INormalizer normalizer, FleetContextBase dbContext) : FleetEntityNormalizer<Intervention>(normalizer)
{
    public override void SetNormalizedContent(Intervention item)
    {
        var contentEntries = GetDefaultNormalizedContentEntries(item);

        var vehicle = item.Vehicle ?? dbContext.Vehicles.Find(item.VehicleId);
        contentEntries.Add(vehicle?.NormalizedTitle);
        var supplier = item.Operator ?? dbContext.InterventionOperators.Find(item.OperatorId);
        contentEntries.Add(supplier?.NormalizedTitle);
        if (item.InterventionType != null || item.InterventionTypeId.HasValue)
        {
            var interventionType = item.InterventionType ?? dbContext.InterventionTypes.Find(item.InterventionTypeId);
            contentEntries.Add(interventionType?.NormalizedTitle);
        }
        if (item.Invoice != null)
        {
            contentEntries.Add(item.Invoice.InvoiceNumber);
            contentEntries.Add(normalizer.Normalize(item.Invoice.Description));
        }

        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}