using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Fleet.Data.PostgreSQL.Extensions;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.DependencyInjection.Entities.Interventions;

public class InterventionPostgresLikeQueryFilter(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Intervention, InterventionSearchObject>
{
    public override IQueryable<Intervention> Build(IQueryable<Intervention> query, InterventionSearchObject? so)
    {
        if (so != null)
        {
            // Q
            query = query.FilterILikeQ(qHelper.Parse(so.Q));
        }

        return query;
    }
}