using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Preppers.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorPrepper(FleetContextBase dbContext) : IEntityPrepper<Operator, int>
{
    public Task Prepare(Operator modified, Operator? original)
    {
        if (original != null)
        {
            // Intervention Types
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
                original.InterventionTypes = (original.InterventionTypes ?? Array.Empty<OperatorInterventionType>())
                    .Except(itemsToRemove)
                    .Concat(itemsToAdd)
                    .ToList();
            }
        }

        return Task.CompletedTask;
    }
}