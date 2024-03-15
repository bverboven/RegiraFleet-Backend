using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Clients;

public class ClientSubscription : IEntityWithSerial, IHasClientId, IHasDescription, IHasStartEndDate
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    [Required]
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }

    public Client? Client { get; set; }
}