using Regira.Entities.Models;

namespace Regira.Fleet.Identity.Models.Clients.Subscriptions;

public class ClientSubscriptionSearchObject : SearchObject
{
    public string? ClientId { get; set; }
}