using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Models.Tenants;

public class TenantInputDto
{
    public string? Id { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    [MaxLength(8)]
    public string? DefaultCulture { get; set; }

    public string? Description { get; set; }


    public ICollection<string>? Languages { get; set; }
}