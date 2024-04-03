using Microsoft.AspNetCore.Identity;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Entities.Users.Claims;

namespace Regira.Fleet.Identity.Entities.Users;

public class FleetUserModel : IEntity<string>
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }

    public string? GivenName { get; set; }
    public string? LastName { get; set; }
    public string? Culture { get; set; }

    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }

    public ICollection<IdentityUserClaim<string>>? UserClaims { get; set; }
    public ICollection<ClientUserClaim>? ClientClaims { get; set; }
    public ICollection<Client>? Clients => ClientClaims
        ?.Where(c => c.Client != null)
        .Select(c => c.Client!)
        .DistinctBy(c => c.Id)
        .ToList();
}