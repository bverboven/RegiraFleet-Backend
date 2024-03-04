using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class InterventionOperatorRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<InterventionOperator, InterventionOperatorSearchObject, EntitySortBy, InterventionOperatorIncludes>(dbContext, appContext)
{
    public override IQueryable<InterventionOperator> Filter(IQueryable<InterventionOperator> query, InterventionOperatorSearchObject? so)
    {
        query = base.Filter(query, so);
        if (so != null)
        {
            query = query.FilterArchivable(so.IsArchived);
            query = query.FilterCode(so.Code);

            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                query = query.Where(x => x.Title!.StartsWith(so.Title, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(so.IdentificationNumber))
            {
                query = query.Where(x => x.IdentificationNumber!.Equals(so.IdentificationNumber, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(so.Phone))
            {
                query = query.Where(x => x.ContactData!.Any(cd => cd.Value == so.Phone));
            }
        }

        return query;
    }

    public override IQueryable<InterventionOperator> AddIncludes(IQueryable<InterventionOperator> query, InterventionOperatorIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(InterventionOperatorIncludes.ContactData))
            {
                query = query
                    .Include(x => x.ContactData!.OrderBy(a => a.SortOrder));
            }
            if (includes.Value.HasFlag(InterventionOperatorIncludes.Addresses))
            {
                query = query
                    .Include(x => x.Addresses!.OrderBy(a => a.SortOrder));
            }
            if (includes.Value.HasFlag(InterventionOperatorIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes);
            }
            if (includes.Value.HasFlag(InterventionOperatorIncludes.Attachments))
            {
                query = query
                    .Include(x => x.Attachments!.OrderBy(a => a.SortOrder));
            }
        }

        return query;
    }

    public override void Modify(InterventionOperator item, InterventionOperator original)
    {
        DbContext.UpdateEntityChildCollection(original, item, x => x.Addresses, (x, collection) => x.Addresses = collection);
        DbContext.UpdateEntityChildCollection(original, item, x => x.ContactData, (x, collection) => x.ContactData = collection);
        DbContext.UpdateEntityChildCollection(original, item, x => x.InterventionTypes, (x, collection) => x.InterventionTypes = collection);

        if (item.Attachments != null)
        {
            DbContext.ModifyEntityAttachments(original, item);
        }

        base.Modify(item, original);
    }
    public override void PrepareItem(InterventionOperator item)
    {
        item.Addresses?.Prepare();
        item.ContactData?.Prepare();

        base.PrepareItem(item);
    }
}