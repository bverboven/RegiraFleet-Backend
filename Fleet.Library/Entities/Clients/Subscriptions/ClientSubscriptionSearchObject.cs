using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Clients.Subscriptions;

public class ClientSubscriptionSearchObject : SearchObject
{
    public int? ClientId { get; set; }
}