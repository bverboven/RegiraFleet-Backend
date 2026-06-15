using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Data.PostgreSQL.Extensions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.DependencyInjection.Postgres;

public class InterventionTypePostgresLikeQueryFilter(FleetContextBase dbContext, IQKeywordHelper qHelper)
    : FilteredQueryBuilderBase<InterventionType, InterventionTypeSearchObject>
{
    public override IQueryable<InterventionType> Build(IQueryable<InterventionType> query,
        InterventionTypeSearchObject? so)
    {
        if (so != null)
        {
            // Title
            query = query.FilterILikeTitleOrCode(qHelper.Parse(so.Title));
            // Q
            query = query.FilterILikeTitleOrCodeQ(qHelper.Parse(so.Q));
        }

        return query;
    }
}