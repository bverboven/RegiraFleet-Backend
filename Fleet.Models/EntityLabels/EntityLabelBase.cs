using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Models.EntityLabels;
public abstract class EntityLabelBase : IEntityLabel, IEntityWithSerial, IHasNormalizedContent
{
    public int Id { get; set; }
    public int ObjectId { get; set; }
    [MaxLength(64)]
    public string? Title { get; set; }
    [MaxLength(512)]
    public string Value { get; set; } = null!;
    [MaxLength(64)]
    public string? LabelType { get; set; }
    public int SortOrder { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }

    [MaxLength(1024)]
    public string? NormalizedContent { get; set; }
}
