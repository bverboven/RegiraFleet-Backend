using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<InterventionType, InterventionTypeSearchObject>(dbContext, appContext)
{
    public override IQueryable<InterventionType> Filter(IQueryable<InterventionType> query, InterventionTypeSearchObject? so)
    {
        query = base.Filter(query, so);
        if (so != null)
        {
            var qHelper = QKeywordHelper.Create();

            query = query.FilterArchivable(so.IsArchived);
            query = query.FilterCode(so.Code);
            query = query.FilterTitle(qHelper.Parse(so.Title));

            if (!string.IsNullOrWhiteSpace(so.Q))
            {
                var kw = qHelper.Parse(so.Q);
                foreach (var q in kw)
                {
                    query = query.Where(x => EF.Functions.Like(x.Code, q.QW) || EF.Functions.Like(x.Title, q.QW));
                }
            }

            // Operator
            if (so.OperatorId?.Any() == true)
            {
                query = query.Where(x => DbContext.InterventionOperators
                    .Where(o => so.OperatorId.Contains(o.Id))
                    .Any(o => o.InterventionTypes!.Any(ot => ot.InterventionTypeId == x.Id))
                );
            }
        }
        return query;
    }

    public override IQueryable<InterventionType> SortBy(IQueryable<InterventionType> query, EntitySortBy? sortBy = null)
    {
        return query.OrderBy(x => x.Title);
    }
}