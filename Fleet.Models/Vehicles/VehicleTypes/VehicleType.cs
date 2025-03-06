using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Models.Abstractions;
using Regira.Fleet.Models.Translations;
using Regira.Normalizing;

namespace Regira.Fleet.Models.Vehicles.VehicleTypes;

public class VehicleType : IFleetEntity, IEntityWithSerial, IHasCode, IHasNormalizedTitle, IHasDescription, IHasTranslations<VehicleTypeTranslation>, IHasTranslations, IArchivable
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    [StringLength(32)]
    public string ClientId { get; set; } = null!;
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? Description { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    [MaxLength(256)]
    [Normalized(SourceProperties = [nameof(Title), nameof(Code)])]
    public string? NormalizedTitle { get; set; }

    public ICollection<VehicleTypeTranslation>? Translations { get; set; }
    ICollection<Translation>? IHasTranslations.Translations
    {
        get => Translations?.Cast<Translation>().ToList();
        set => Translations = value?.Cast<VehicleTypeTranslation>().ToList();
    }
}