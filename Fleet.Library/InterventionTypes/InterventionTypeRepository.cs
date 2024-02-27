using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Fleet.DAL.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.InterventionTypes;

public class InterventionTypeRepository(FleetContext dbContext) : DefaultFleetRepository<InterventionType, InterventionTypeSearchObject>(dbContext)
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
        }
        return query;
    }
}