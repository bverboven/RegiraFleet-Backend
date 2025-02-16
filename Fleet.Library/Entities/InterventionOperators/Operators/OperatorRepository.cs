using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorRepository(FleetContextBase dbContext, IFleetAppContext appContext,
    IQueryBuilder<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes> queryBuilder)
    : FleetRepositoryBase<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes>(dbContext, queryBuilder, appContext)
{
    public override void Modify(Operator item, Operator original)
    {
        // Addresses
        DbContext.UpdateEntityChildCollection(original, item, x => x.Addresses, (x, collection) => x.Addresses = collection);
        // Contact Data
        DbContext.UpdateEntityChildCollection(original, item, x => x.ContactData, (x, collection) => x.ContactData = collection);
        // Labels
        DbContext.UpdateEntityChildCollection(original, item, x => x.Labels, (x, collection) => x.Labels = collection);

        // Intervent Types
        if (item.InterventionTypes != null)
        {
            var itemsToRemove = original.InterventionTypes?
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
            original.InterventionTypes = (original.InterventionTypes ?? Array.Empty<OperatorInterventionType>())
                .Except(itemsToRemove)
                .Concat(itemsToAdd)
                .ToList();
        }
        // Attachments
        if (item.Attachments != null)
        {
            DbContext.ModifyEntityAttachments(original, item);
        }

        base.Modify(item, original);
    }
    public override void PrepareItem(Operator item)
    {
        base.PrepareItem(item);

        item.Addresses?.Prepare();
        item.ContactData?.Prepare();
        item.Labels?.Prepare();
    }
}