using System.ComponentModel.DataAnnotations;
using Regira.Fleet.Models.Translations;

namespace Regira.Fleet.Models.InterventionTypes;

public class InterventionTypeInputDto
{
    public int Id { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; } = null!;
    [Required]
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsArchived { get; set; }

    public ICollection<TranslationDto>? Translations { get; set; }
}