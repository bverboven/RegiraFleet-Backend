using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Api.Models.Input;

public class CarInputDto
{
    public int? BrandId { get; set; }
    public int? CarTypeId { get; set; }
    [Required]
    [MaxLength(8)]
    public string Code { get; set; } = null!;
    [MaxLength(64)]
    public string? Model { get; set; }
    public bool IsArchived { get; set; }
}