using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionLikeQueryFilter(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Intervention, InterventionSearchObject>
{
    public override IQueryable<Intervention> Build(IQueryable<Intervention> query, InterventionSearchObject? so)
    {
        if (so != null)
        {
            // Q
            query = query.FilterLikeQ(qHelper.Parse(so.Q));
        }

        return query;
    }
}