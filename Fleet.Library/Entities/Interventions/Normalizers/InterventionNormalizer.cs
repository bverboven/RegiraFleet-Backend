using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.EntityLabels;
using Regira.Fleet.Models.Interventions;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Interventions.Normalizers;

public class InterventionNormalizer(INormalizer normalizer, EntityLabelNormalizer labelNormalizer, FleetContextBase dbContext) : FleetEntityNormalizer<Intervention>(normalizer)
{
    public override void HandleNormalize(Intervention? item)
    {
        if (item != null)
        {
            labelNormalizer.NormalizeItem(item);
        }

        base.HandleNormalize(item);
    }

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
            contentEntries.Add(DefaultNormalizer.Normalize(item.Invoice.Description));
        }
        
        if (item.Labels?.Any() == true)
        {
            contentEntries.AddRange(item.Labels.Select(a => a.NormalizedContent));
        }

        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}