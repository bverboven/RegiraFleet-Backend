using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Entities.Users.Claims;

public class ClientUserClaim : IEntityWithSerial, IHasClientId, IHasUserId
{
    public int Id { get; set; }
    [StringLength(64)]
    public string ClientId { get; set; } = null!;
    [StringLength(64)]
    public string UserId { get; set; } = null!;
    [MaxLength(64)]
    public string ClaimType { get; set; } = null!;
    [MaxLength(256)]
    public string? ClaimValue { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}
