using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorFilteredQueryBuilder(FleetContextBase dbContext, IQKeywordHelper qHelper)
    : FilteredQueryBuilderBase<Operator, OperatorSearchObject>
{
    public override IQueryable<Operator> Build(IQueryable<Operator> query, OperatorSearchObject? so)
    {
        if (so != null)
        {
            // Code
            query = query.FilterCode(so.Code);

            // IdentificationNumber
            if (!string.IsNullOrWhiteSpace(so.IdentificationNumber))
            {
                query = query.Where(x => x.IdentificationNumber!.Equals(so.IdentificationNumber));
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var keywords = qHelper.Parse(so.Title);
                query = query.FilterLikeTitleOrCode(keywords);
            }
            // InterventionTypeId
            if (so.InterventionTypeId?.Any() == true)
            {
                query = query.Where(x => so.InterventionTypeId.All(id => x.InterventionTypes!.Any(ot => ot.InterventionTypeId == id)));
            }
            // HasIntervention
            if (so.HasIntervention.HasValue)
            {
                query = query.Where(x => dbContext.Interventions.Any(i => i.OperatorId == x.Id));
            }
        }

        return query;
    }
}