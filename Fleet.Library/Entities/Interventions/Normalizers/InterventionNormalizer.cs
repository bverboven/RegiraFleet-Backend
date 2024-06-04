using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Interventions.Normalizers;

public class InterventionNormalizer(INormalizer normalizer, EntityLabelNormalizer labelNormalizer, FleetContextBase dbContext) : FleetEntityNormalizer<Intervention>(normalizer)
{
    private List<Vehicle> _vehicles = null!;
    private List<Operator> _suppliers = null!;
    private List<InterventionType> _interventionTypes = null!;

    public override async Task HandleNormalizeMany(IEnumerable<Intervention?> items, bool recursive = false)
    {
        var vehicleIds = items.Select(x => x?.VehicleId).Where(id => id.HasValue).Distinct().ToArray();
        _vehicles = await dbContext.Vehicles.Where(x => vehicleIds.Contains(x.Id)).ToListAsync();
        var supplierIds = items.Select(x => x?.OperatorId).Where(id => id.HasValue).Distinct().ToArray();
        _suppliers = await dbContext.InterventionOperators.Where(x => supplierIds.Contains(x.Id)).ToListAsync();
        var typeIds = items.Select(x => x?.InterventionTypeId).Where(id => id.HasValue).Distinct().ToArray();
        _interventionTypes = await dbContext.InterventionTypes.Where(x => typeIds.Contains(x.Id)).ToListAsync();

        await base.HandleNormalizeMany(items, recursive);
    }
    public override void HandleNormalize(Intervention? item, bool recursive = false)
    {
        if (item == null)
        {
            return;
        }

        labelNormalizer.NormalizeItem(item);

        base.HandleNormalize(item);

        var contentEntries = GetDefaultNormalizedContentEntries(item);

        var vehicle = item.Vehicle ?? _vehicles.Find(x => x.Id == item.VehicleId);
        contentEntries.Add(vehicle?.NormalizedTitle);
        var supplier = item.Operator ?? _suppliers.Find(x => x.Id == item.OperatorId);
        contentEntries.Add(supplier?.NormalizedTitle);
        if (item.InterventionType != null || item.InterventionTypeId.HasValue)
        {
            var interventionType = item.InterventionType ?? _interventionTypes.Find(x => x.Id == item.InterventionTypeId);
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