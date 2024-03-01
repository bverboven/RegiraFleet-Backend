using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Entities.Abstractions;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.InterventionOperators.Addresses;
using Regira.Fleet.Entities.InterventionOperators.ContactData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

// InterventionOperator ??
public class InterventionOperator : IFleetEntity, IEntityWithSerial, IHasCode, IHasNormalizedTitle, IHasDescription, IHasNormalizedContent, IArchivable, IHasAttachments<InterventionOperatorAttachment>, IHasAttachments
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    public int ClientId { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(128)]
    public string Title { get; set; } = null!;

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

    public ICollection<InterventionType>? InterventionTypes { get; set; }
    public ICollection<Address>? Addresses { get; set; }
    public ICollection<InterventionOperatorContactData>? ContactData { get; set; }

    [NotMapped]
    public bool? HasAttachment { get; set; }
    public ICollection<InterventionOperatorAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<InterventionOperatorAttachment>().ToList();
    }


    [MaxLength(256)]
    public string? NormalizedTitle { get; set; }
    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }
}