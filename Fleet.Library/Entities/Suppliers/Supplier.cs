using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Entities.Abstractions;
using Regira.Fleet.Entities.Suppliers.Addresses;
using Regira.Fleet.Entities.Suppliers.ContactData;
using Regira.Fleet.Entities.Suppliers.SupplierTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Entities.Suppliers;

public class Supplier : IFleetEntity, IEntityWithSerial, IHasCode, IHasNormalizedTitle, IHasDescription, IHasNormalizedContent, IArchivable, IHasAttachments<SupplierAttachment>, IHasAttachments
{
    public int Id { get; set; }
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    public int ClientId { get; set; }
    public int? SupplierTypeId { get; set; }
    [MaxLength(16)]
    public string? Code { get; set; }
    [MaxLength(128)]
    public string? Title { get; set; }

    [MaxLength(32)]
    public string? IdentificationNumber { get; set; }
    [MaxLength(32)]
    public string? NormalizedIdentificationNumber { get; set; }

    [MaxLength(512)]
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public SupplierType? SupplierType { get; set; }
    public ICollection<Address>? Addresses { get; set; }
    public ICollection<SupplierContactData>? ContactData { get; set; }

    [NotMapped]
    public bool? HasAttachment { get; set; }
    public ICollection<SupplierAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<SupplierAttachment>().ToList();
    }


    [MaxLength(256)]
    public string? NormalizedTitle { get; set; }
    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }
}