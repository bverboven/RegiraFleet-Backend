using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Fleet.Identity.Models.Clients;

namespace Regira.Fleet.Identity.Entities.Clients;

public class ClientIncludableQueryBuilder : IIncludableQueryBuilder<Client, string, ClientIncludes>
{
    public IQueryable<Client> AddIncludes(IQueryable<Client> query, ClientIncludes? includes = null)
    {
        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(ClientIncludes.Languages))
            {
                query = query.Include(x => Enumerable.OrderBy<ClientLanguage, string>(x.Languages!, a => a.LangCode));
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