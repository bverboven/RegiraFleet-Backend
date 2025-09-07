using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Fleet.Identity.Models.Tenants;

namespace Regira.Fleet.Identity.Entities.Tenants;

public class TenantFilteredQueryBuilder(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Tenant, string, TenantSearchObject>
{
    public override IQueryable<Tenant> Build(IQueryable<Tenant> query, TenantSearchObject? so)
    {
        if (so != null)
        {
            // ID
            if (!string.IsNullOrWhiteSpace(so.Id))
            {
                query = query.Where(x => x.Id == so.Id);
            }
            // Code
            if (!string.IsNullOrWhiteSpace(so.Code))
            {
                var upperCode = so.Code.ToUpper();
                query = query.Where(x => x.Code!.ToUpper() == upperCode);
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var qTitles = qHelper.Parse(so.Title);
                foreach (var q in qTitles)
                {
                    query = query.Where(x => x.Code!.ToUpper() == q.Normalized || EF.Functions.Like(x.NormalizedTitle, q.Q));
                }
            }
            // Culture
            if (!string.IsNullOrWhiteSpace(so.Culture))
            {
                query = query.Where(x => x.DefaultCulture == so.Culture);
            }
            // Q
            if (!string.IsNullOrWhiteSpace(so.Q))
            {
                var keywords = qHelper.Parse(so.Q);
                foreach (var q in keywords)
                {
                    query = query.Where(x => EF.Functions.Like(x.NormalizedTitle, q.QW));
                }
            }
        }

        return query;
    }
}