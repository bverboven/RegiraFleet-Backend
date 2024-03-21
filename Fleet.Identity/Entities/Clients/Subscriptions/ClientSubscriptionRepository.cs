using Regira.Entities.EFcore.Services;
using Regira.Fleet.Identity.Data;

namespace Regira.Fleet.Identity.Entities.Clients.Subscriptions;

public class ClientSubscriptionRepository(AccountsContext dbContext) : EntityRepository<AccountsContext, ClientSubscription, int, ClientSubscriptionSearchObject>(dbContext)
{
}
