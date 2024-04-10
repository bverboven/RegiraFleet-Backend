using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Models.Vehicles.Brands;

public class BrandTranslation : IEntity, IHasNormalizedTitle, IHasCulture
{
    public int BrandId { get; set; }
    [MaxLength(8)]
    public string Culture { get; set; } = null!;

    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;

    [MaxLength(256)]
    [Normalized(SourceProperties = new[] { nameof(Title), nameof(Code) })]
    public string? NormalizedTitle { get; set; }
}