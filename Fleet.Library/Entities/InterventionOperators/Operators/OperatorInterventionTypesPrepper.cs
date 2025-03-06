using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Preppers.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorInterventionTypesPrepper(FleetContextBase dbContext) : EntityPrepperBase<Operator>
{
    public override Task Prepare(Operator modified, Operator? original)
    {
        if (original != null)
        {
            if (modified.InterventionTypes != null)
            {
                var itemsToRemove = original.InterventionTypes?
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
                    dbContext.Entry(itemToAdd).State = EntityState.Added;
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