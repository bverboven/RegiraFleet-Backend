using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeLikeQueryFilter(FleetContextBase dbContext, IQKeywordHelper qHelper)
    : FilteredQueryBuilderBase<InterventionType, InterventionTypeSearchObject>
{
    public override IQueryable<InterventionType> Build(IQueryable<InterventionType> query,
        InterventionTypeSearchObject? so)
    {
        if (so != null)
        {
            // Title
            query = query.FilterLikeTitleOrCode(qHelper.Parse(so.Title));
            // Q
            query = query.FilterLikeTitleOrCodeQ(qHelper.Parse(so.Q));
        }

        return query;
    }
}