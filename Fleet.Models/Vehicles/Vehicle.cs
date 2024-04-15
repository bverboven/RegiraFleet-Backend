using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Models.Abstractions;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Models.Vehicles;

public class Vehicle : IFleetEntity, IEntityWithSerial, IHasCode, IArchivable, IHasDescription, IHasNormalizedTitle, IHasNormalizedContent,
    IHasLabels<VehicleLabel>, IHasLabels, IHasAttachments, IHasAttachments<VehicleAttachment>
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    [StringLength(32)]
    public string ClientId { get; set; } = null!;
    public int? BrandId { get; set; }
    public int? VehicleTypeId { get; set; }
    [Required]
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string? Model { get; set; }

    [MaxLength(64)]
    public string? IdentificationNumber { get; set; }
    [MaxLength(64)]
    [Normalized(SourceProperty = nameof(IdentificationNumber))]
    public string? NormalizedIdentificationNumber { get; set; }

    public string? Description { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public virtual Brand? Brand { get; set; }
    public virtual VehicleType? VehicleType { get; set; }
    public ICollection<VehicleInterventionType>? InterventionTypes { get; set; }
    //public ICollection<Intervention>? Interventions { get; set; }

    // Labels
    public ICollection<VehicleLabel>? Labels { get; set; }
    ICollection<IEntityLabel>? IHasLabels.Labels
    {
        get => Labels?.Cast<IEntityLabel>().ToList();
        set => Labels = value?.Cast<VehicleLabel>().ToList();
    }


    [NotMapped]
    public bool? HasAttachment { get; set; }
    public ICollection<VehicleAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<VehicleAttachment>().ToList();
    }

    public string? Title => Model;
    [MaxLength(256)]
    public string? NormalizedTitle { get; set; }
    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }

}