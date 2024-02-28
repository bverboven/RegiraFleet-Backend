using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Models;

public class FleetUser : IdentityUser
{
    [PersonalData]
    [MaxLength(8)]
    public string? Culture { get; set; }
    [PersonalData]
    [MaxLength(8)]
    public string? UICulture { get; set; }
}