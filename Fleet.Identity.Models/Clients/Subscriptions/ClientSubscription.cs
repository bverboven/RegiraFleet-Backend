using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Models.Clients.Subscriptions;

public class ClientSubscription : IEntityWithSerial, IHasClientId, IHasDescription, IHasStartEndDate
{
    public int Id { get; set; }
    public string ClientId { get; set; } = null!;
    [Required]
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }


    public Client? Client { get; set; }
}
