using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Abstractions;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeTranslation : IEntity, IHasNormalizedTitle, IHasLangCode
{
    public int InterventionTypeId { get; set; }
    public string LangCode { get; set; } = null!;

    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    
    [MaxLength(256)]
    [Normalized(SourceProperties = new[] { nameof(Title), nameof(Code) })]
    public string? NormalizedTitle { get; set; }
}