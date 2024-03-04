using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeInputDto
{
    public int Id { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; } = null!;
    [Required]
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    public bool IsArchived { get; set; }
}