using Microsoft.EntityFrameworkCore;
using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.Services;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleWriteService(FleetContextBase dbContext, IEntityReadService<Vehicle, int> readService)
    : EntityWriteService<FleetContextBase, Vehicle, int>(dbContext, readService)
{
    public override async Task<Vehicle?> Modify(Vehicle item)
    {
        item.Labels?.Prepare();

        var original = await base.Modify(item);
        if (item.InterventionTypes != null)
        {
            var itemsToRemove = original!.InterventionTypes?
                .Where(o => item.InterventionTypes.All(x => o.InterventionTypeId != x.InterventionTypeId))
                .ToArray() ?? [];
            var itemsToAdd = item.InterventionTypes
                .Where(x => original.InterventionTypes == null || original.InterventionTypes.All(o => x.InterventionTypeId != o.InterventionTypeId))
                .ToArray();
            foreach (var itemToRemove in itemsToRemove)
            {
                DbContext.Entry(itemToRemove).State = EntityState.Deleted;
            }
            foreach (var itemToAdd in itemsToAdd)
            {
                DbContext.Entry(itemToAdd).State = EntityState.Added;
            }
            original.InterventionTypes = (original.InterventionTypes ?? Array.Empty<VehicleInterventionType>())
                .Except(itemsToRemove)
                .Concat(itemsToAdd)
                .ToList();
        }

        DbContext.UpdateEntityChildCollection(original!, item, x => x.Labels, (x, collection) => x.Labels = collection);

        if (item.Attachments != null)
        {
            DbContext.ModifyEntityAttachments(original!, item);
        }

        return original;
    }
}