using Regira.Entities.EFcore.Services;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Clients.Subscriptions;

namespace Regira.Fleet.Identity.Entities.Clients.Subscriptions;

public class ClientSubscriptionRepository(AccountsContextBase dbContext) : EntityRepository<AccountsContextBase, ClientSubscription, int, ClientSubscriptionSearchObject>(dbContext)
{
}
