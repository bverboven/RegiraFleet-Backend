using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Clients.Users;

public class ClientUserClaim : IEntityWithSerial, IHasCreated
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    [MaxLength(255)]
    public string UserId { get; set; } = null!;
    [MaxLength(64)]
    public string ClaimType { get; set; } = null!;
    [MaxLength(256)]
    public string? ClaimValue { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}
