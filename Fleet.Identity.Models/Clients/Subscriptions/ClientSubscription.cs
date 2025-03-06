using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Identity.Models.Clients.Subscriptions;

public class ClientSubscription : IEntityWithSerial, IHasClientId, IHasDescription, IHasStartEndDate
{
    public int Id { get; set; }
    [StringLength(32)]
    public string ClientId { get; set; } = null!;
    [Required]
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? Description { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }


    public Client? Client { get; set; }
}
