using Microsoft.AspNetCore.Identity;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Models.Clients;
using Regira.Fleet.Identity.Models.Users.Claims;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Identity.Models.Users;

public class FleetUser : IdentityUser, IEntity<string>
{
    [PersonalData]
    [MaxLength(64)]
    public string? GivenName { get; set; }
    [PersonalData]
    [MaxLength(64)]
    public string? LastName { get; set; }
    [PersonalData]
    [MaxLength(8)]
    public string? Culture { get; set; }

    [NotMapped]
    public string? CurrentPassword { get; set; }
    [NotMapped]
    public string? NewPassword { get; set; }

    public ICollection<IdentityUserClaim<string>>? UserClaims { get; set; }
    public ICollection<ClientUserClaim>? ClientClaims { get; set; }

    [NotMapped]
    public ICollection<Client>? Clients => ClientClaims
        ?.Where(c => c.Client != null)
        .Select(c => c.Client!)
        .DistinctBy(c => c.Id)
        .ToList();
}