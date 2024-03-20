using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Clients.Subscriptions;

public class ClientSubscriptionRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<ClientSubscription, ClientSubscriptionSearchObject>(dbContext, appContext)
{
}
