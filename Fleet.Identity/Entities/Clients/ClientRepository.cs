using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords;
using Regira.Entities.Models;
using Regira.Fleet.Identity.Abstractions;
using Regira.Fleet.Identity.Data;

namespace Regira.Fleet.Identity.Entities.Clients;
public class ClientRepository(AccountsContext dbContext) : IdentityRepositoryBase<Client, string, ClientSearchObject, EntitySortBy, ClientIncludes>(dbContext)
{
    public override IQueryable<Client> Filter(IQueryable<Client> query, ClientSearchObject? so)
    {
        if (so != null)
        {
            var qHelper = QKeywordHelper.Create();
            if (!string.IsNullOrWhiteSpace(so.Id))
            {
                query = query.Where(x => x.Id == so.Id);
            }
            if (!string.IsNullOrWhiteSpace(so.Q))
            {
                var keywords = qHelper.Parse(so.Q);
                foreach (var q in keywords)
                {
                    query = query.Where(x => EF.Functions.ILike(x.Code!, q.Keyword!) || EF.Functions.ILike(x.NormalizedTitle!, q.QW!));
                }
            }
        }

        return query;
    }
    public override IQueryable<Client> AddIncludes(IQueryable<Client> query, ClientIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

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
    public override Task Add(Client item)
    {
        item.Id ??= Guid.NewGuid().ToString("N");
        return base.Add(item);
    }
    public override void Modify(Client item, Client original)
    {
        base.Modify(item, original);

        //DbContext.UpdateEntityChildCollection(original, item, x => x.Languages, (x, collection) => x.Languages = collection);
        //DbContext.UpdateEntityChildCollection(original, item, x => x.Subscriptions, (x, collection) => x.Subscriptions = collection);
    }
    public override void PrepareItem(Client item)
    {
        base.PrepareItem(item);

        //item.Subscriptions?.Prepare();
    }
}