using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Identity.Models.Clients;

namespace Regira.Fleet.Identity.Entities.Clients;

public class ClientQueryBuilder(IEnumerable<IGlobalFilteredQueryBuilder> globalFilters, IQKeywordHelper qHelper) 
    : QueryBuilder<Client, string, ClientSearchObject, EntitySortBy, ClientIncludes>(globalFilters)
{
    public override IQueryable<Client> Filter(IQueryable<Client> query, ClientSearchObject? so)
    {
        query = base.Filter(query, so);

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

    public override IQueryable<Client> AddIncludes(IQueryable<Client> query, IList<ClientSearchObject?>? so, IList<EntitySortBy>? sortByList, ClientIncludes? includes)
    {
        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(ClientIncludes.Languages))
            {
                query = query.Include(x => x.Languages!.OrderBy(a => a.LangCode));
            }
            if (includes.Value.HasFlag(ClientIncludes.Subscriptions))
            {
                query = query.Include(x => x.Subscriptions!.OrderByDescending(s => s.EndDate ?? s.StartDate ?? s.Created));
            }
            else if (includes.Value.HasFlag(ClientIncludes.ActiveSubscription))
            {
                query = query.Include(x => x.Subscriptions!.Where(s => s.StartDate <= DateTime.Now && (s.EndDate == null || s.EndDate >= DateTime.Today)));
            }
        }

        return query;
    }
}