using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleTypeTranslation : IEntity, IHasNormalizedTitle, IHasLangCode
{
    public int VehicleTypeId { get; set; }
    [StringLength(2)]
    public string LangCode { get; set; } = null!;

    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    
    [MaxLength(256)]
    [Normalized(SourceProperties = new[] { nameof(Title), nameof(Code) })]
    public string? NormalizedTitle { get; set; }
}
