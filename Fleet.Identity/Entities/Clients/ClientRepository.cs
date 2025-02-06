using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Identity.Abstractions;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Clients;

namespace Regira.Fleet.Identity.Entities.Clients;

public class ClientRepository(AccountsContextBase dbContext,
    IQueryBuilder<Client, string, ClientSearchObject, EntitySortBy, ClientIncludes> queryBuilder)
    : IdentityRepositoryBase<Client, string, ClientSearchObject, EntitySortBy, ClientIncludes>(dbContext, queryBuilder)
{
    public override Task Add(Client item)
    {
        if (string.IsNullOrWhiteSpace(item.Id))
        {
            item.Id = Guid.NewGuid().ToString("N");
        }

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