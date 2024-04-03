using Regira.Fleet.Identity.Entities.Users.Claims;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Entities.Users;

public class FleetUserInputDto
{
    public string? Id { get; set; } = null!;
    [MaxLength(256)]
    public string? UserName { get; set; } = null!;
    [MaxLength(256)]
    public string? NewPassword { get; set; }
    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    [MaxLength(64)]
    public string? GivenName { get; set; }
    [MaxLength(64)]
    public string? LastName { get; set; }
    [MaxLength(8)]
    public string? Culture { get; set; }

    public ICollection<UserClaimDto>? UserClaims { get; set; }
    public ICollection<ClientUserClaimDto>? ClientClaims { get; set; }
}
