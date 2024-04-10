using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Models.Translations;

public class TranslationDto
{
    public int Id { get; set; }
    [MaxLength(8)]
    public string Culture { get; set; } = null!;
    [MaxLength(64)]
    public string Title { get; set; } = null!;
}
