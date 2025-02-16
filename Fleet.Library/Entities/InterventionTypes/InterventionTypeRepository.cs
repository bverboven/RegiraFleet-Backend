using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeRepository(FleetContextBase dbContext,
    IQueryBuilder<InterventionType, InterventionTypeSearchObject, EntitySortBy, EntityIncludes> queryBuilder, IFleetAppContext appContext)
    : FleetRepositoryBase<InterventionType, InterventionTypeSearchObject>(dbContext, queryBuilder, appContext)
{
    public override IQueryable<InterventionType> Query(IQueryable<InterventionType> query, IList<InterventionTypeSearchObject?> searchObjects, IList<EntitySortBy> sortBy, EntityIncludes? includes, PagingInfo? pagingInfo)
    {
        query = query
            .Include(x => x.Translations)
            .OrderBy(x => x.Title);

        return base.Query(query, searchObjects, sortBy, includes, pagingInfo);
    }

    public override void Modify(InterventionType item, InterventionType original)
    {
        base.Modify(item, original);

        DbContext.UpdateEntityChildCollection(original, item, x => x.Translations, (x, collection) => x.Translations = collection);
    }
    public override void PrepareItem(InterventionType item)
    {
        base.PrepareItem(item);

        item.Translations?.Prepare();
    }
}