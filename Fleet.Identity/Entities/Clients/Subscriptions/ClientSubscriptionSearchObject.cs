using Regira.Entities.Models;

namespace Regira.Fleet.Identity.Entities.Clients.Subscriptions;

public class ClientSubscriptionSearchObject : SearchObject
{
    public string? ClientId { get; set; }
}