using Microsoft.EntityFrameworkCore;
using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Identity.Entities.Users;

public static class FleetUserCollectionExtensions
{
    public static void UpdateEntityChildCollection<TEntity, TEntityKey, TChild, TChildKey>(this DbContext dbContext, TEntity original, TEntity modified, Func<TEntity, ICollection<TChild>?> childrenGetter, Action<TEntity, ICollection<TChild>> childrenSetter, Action<TChild?, TChild>? processExtra = null)
    where TChild : class, IEntity<TChildKey>
    {
        var originalChildCollection = childrenGetter(original);
        var modifiedChildCollection = childrenGetter(modified);
        // ignore when no child collection is passed for either original OR modified entity
        if (originalChildCollection == null || modifiedChildCollection == null)
        {
            return;
        }

        var childrenToRemove = originalChildCollection!.Where(oc => modifiedChildCollection.All(c => !oc.Id!.Equals(c.Id)));
        var childrenToAdd = modifiedChildCollection!.Where(c => originalChildCollection.All(oc => !oc.Id!.Equals(c.Id)));
        var childrenToUpdate = originalChildCollection.Except(childrenToRemove);

        if (childrenToRemove.Any())
        {
            dbContext.RemoveRange(childrenToRemove);
        }
        if (childrenToAdd.Any())
        {
            foreach (var child in childrenToAdd)
            {
                processExtra?.Invoke(null, child);
                dbContext.Add(child);
            }
        }
        if (childrenToUpdate?.Any() == true)
        {
            foreach (var originalChild in childrenToUpdate)
            {
                var modifiedChild = modifiedChildCollection.First(c => c.Id!.Equals(originalChild.Id));
                processExtra?.Invoke(originalChild, modifiedChild);
                var childEntry = dbContext.Entry(originalChild);
                childEntry.CurrentValues.SetValues(modifiedChild);
                childEntry.State = EntityState.Modified;
            }
        }

        childrenSetter(original, modifiedChildCollection);
    }
}