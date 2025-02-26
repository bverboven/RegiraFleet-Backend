using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Models.Translations;

public class Translation : IHasNormalizedTitle, IHasCulture
{
    public int Id { get; set; }
    public int ObjectId { get; set; }
    [MaxLength(8)]
    public string Culture { get; set; } = null!;

    [MaxLength(64)]
    public virtual string Title { get; set; } = null!;

    [MaxLength(64)]
    [Normalized(SourceProperty = nameof(Title))]
    public virtual string? NormalizedTitle { get; set; }
}
