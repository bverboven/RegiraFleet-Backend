using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Entities.Users.Claims;

namespace Regira.Fleet.Identity.Entities.Users;

public class FleetUserDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }

    public string? GivenName { get; set; }
    public string? LastName { get; set; }
    public string? Culture { get; set; }

    public ICollection<UserClaimDto>? UserClaims { get; set; }
    public ICollection<ClientUserClaimDto>? ClientClaims { get; set; }
    public ICollection<ClientDto>? Clients { get; set; }
}
