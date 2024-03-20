using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Identity.Models;

public class FleetUser : IdentityUser
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
    public ICollection<IdentityRole>? UserRoles { get; set; }
}