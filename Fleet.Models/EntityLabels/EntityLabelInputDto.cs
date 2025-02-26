using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Models.EntityLabels;

public class EntityLabelInputDto
{
    public int Id { get; set; }
    public int ObjectId { get; set; }
    [MaxLength(64)]
    public string? Title { get; set; }
    [Required]
    [MaxLength(512)]
    public string Value { get; set; } = null!;
    [MaxLength(64)]
    public string? LabelType { get; set; }
    public int SortOrder { get; set; }
}
