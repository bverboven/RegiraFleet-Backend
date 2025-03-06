using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Preppers.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleInterventionTypesPrepper(FleetContextBase dbContext) : EntityPrepperBase<Vehicle>
{
    public override Task Prepare(Vehicle modified, Vehicle? original)
    {
        if (original != null)
        {
            if (modified.InterventionTypes != null)
            {
                var itemsToRemove = original!.InterventionTypes?
                    .Where(o => modified.InterventionTypes.All(x => o.InterventionTypeId != x.InterventionTypeId))
                    .ToArray() ?? [];
                var itemsToAdd = modified.InterventionTypes
                    .Where(x => original.InterventionTypes == null || original.InterventionTypes.All(o => x.InterventionTypeId != o.InterventionTypeId))
                    .ToArray();
                foreach (var itemToRemove in itemsToRemove)
                {
                    dbContext.Entry(itemToRemove).State = EntityState.Deleted;
                }
                foreach (var itemToAdd in itemsToAdd)
                {
                    dbContext.Add(itemToAdd);
                }
                foreach (var item in modified.InterventionTypes.Except(itemsToAdd))
                {
                    dbContext.Entry(item).State = EntityState.Modified;
                }
            }
        }

        return Task.CompletedTask;
    }
}