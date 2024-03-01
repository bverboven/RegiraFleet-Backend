using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Vehicles.Brands;

public class BrandInputDto
{
    [MaxLength(8)]
    public string? Code { get; set; }
    [Required]
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    public bool IsArchived { get; set; }
}