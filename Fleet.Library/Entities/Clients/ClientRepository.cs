using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;

namespace Regira.Fleet.Entities.Clients;
public class ClientRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<Client, ClientSearchObject, EntitySortBy, ClientIncludes>(dbContext, appContext)
{
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
    public override void Modify(Client item, Client original)
    {
        base.Modify(item, original);

        //DbContext.UpdateEntityChildCollection(original, item, x => x.Languages, (x, collection) => x.Languages = collection);
        DbContext.UpdateEntityChildCollection(original, item, x => x.Subscriptions, (x, collection) => x.Subscriptions = collection);
    }
    public override void PrepareItem(Client item)
    {
        base.PrepareItem(item);

        item.Subscriptions?.Prepare();
    }
}
