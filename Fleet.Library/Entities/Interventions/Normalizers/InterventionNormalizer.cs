using Microsoft.EntityFrameworkCore;
using Regira.Entities.Normalizing.Abstractions;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Interventions.Normalizers;

public class InterventionNormalizer(INormalizer normalizer, IEntityNormalizer<IEntityLabel> labelNormalizer, FleetContextBase dbContext)
    : FleetEntityNormalizer<Intervention>(normalizer)
{
    private List<Vehicle> _vehicles = null!;
    private List<Operator> _suppliers = null!;
    private List<InterventionType> _interventionTypes = null!;

    public override async Task HandleNormalizeMany(IEnumerable<Intervention> items, CancellationToken cancellationToken = default)
    {
        var itemList = items as Intervention[] ?? items.ToArray();
        var vehicleIds = itemList.Select(x => x.VehicleId).Distinct().ToArray();
        _vehicles = await dbContext.Vehicles.Where(x => vehicleIds.Contains(x.Id)).ToListAsync(cancellationToken);
        var supplierIds = itemList.Select(x => x.OperatorId).Distinct().ToArray();
        _suppliers = await dbContext.InterventionOperators.Where(x => supplierIds.Contains(x.Id)).ToListAsync(cancellationToken);
        var typeIds = itemList.Select(x => x.InterventionTypeId).Where(id => id.HasValue).Distinct().ToArray();
        _interventionTypes = await dbContext.InterventionTypes.Where(x => typeIds.Contains(x.Id)).ToListAsync(cancellationToken);

        await base.HandleNormalizeMany(itemList, cancellationToken);
    }
    public override async Task HandleNormalize(Intervention item, CancellationToken cancellationToken = default)
    {
        if (item.Labels?.Any() == true)
        {
            await labelNormalizer.HandleNormalizeMany(item.Labels, cancellationToken);
        }

        await base.HandleNormalize(item, cancellationToken);

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
            contentEntries.Add(DefaultPropertyNormalizer.Normalize(item.Invoice.Description));
        }

        if (item.Labels?.Any() == true)
        {
            contentEntries.AddRange(item.Labels.Select(a => a.NormalizedContent));
        }

        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}