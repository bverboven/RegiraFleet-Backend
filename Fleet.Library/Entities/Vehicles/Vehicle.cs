using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Entities.Vehicles;

public class Vehicle : IFleetEntity, IEntityWithSerial, IHasCode, IArchivable, IHasAttachments<VehicleAttachment>, IHasAttachments, IHasNormalizedTitle, IHasNormalizedContent
{
    public int Id { get; set; }
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    public int ClientId { get; set; }
    public int? BrandId { get; set; }
    public int? VehicleTypeId { get; set; }
    [Required]
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string? Model { get; set; }
    public string? Notes { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public virtual Brand? Brand { get; set; }
    public virtual VehicleType? VehicleType { get; set; }


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