using Microsoft.AspNetCore.Identity;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Entities.Users.Claims;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Identity.Entities.Users;

public class FleetUser : IdentityUser, IEntity<string>
{
    [PersonalData]
    [MaxLength(8)]
    public string? Culture { get; set; }
    [PersonalData]
    [MaxLength(8)]
    public string? UICulture { get; set; }

    [NotMapped]
    public string? CurrentPassword { get; set; }
    [NotMapped]
    public string? NewPassword { get; set; }

    public ICollection<IdentityUserClaim<string>>? UserClaims { get; set; }
    public ICollection<ClientUserClaim>? ClientClaims { get; set; }

    [NotMapped]
    public ICollection<Client>? Clients { get; set; }
}