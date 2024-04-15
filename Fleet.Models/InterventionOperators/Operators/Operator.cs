using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Models.Abstractions;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Addresses;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Models.InterventionOperators.Operators;

// InterventionOperator ??
public class Operator : IFleetEntity, IEntityWithSerial, IHasCode, IHasNormalizedTitle, IHasDescription, IHasNormalizedContent, IArchivable, 
    IHasLabels<OperatorLabel>, IHasLabels, IHasAttachments<OperatorAttachment>, IHasAttachments
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    [StringLength(32)]
    public string ClientId { get; set; } = null!;
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(128)]
    public string Title { get; set; } = null!;

    [MaxLength(64)]
    public string? IdentificationNumber { get; set; }

    public string? Description { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public ICollection<OperatorAddress>? Addresses { get; set; }
    public ICollection<OperatorContactData>? ContactData { get; set; }
    public ICollection<OperatorInterventionType>? InterventionTypes { get; set; }

    // Labels
    public ICollection<OperatorLabel>? Labels { get; set; }
    ICollection<IEntityLabel>? IHasLabels.Labels
    {
        get => Labels?.Cast<IEntityLabel>().ToList();
        set => Labels = value?.Cast<OperatorLabel>().ToList();
    }

    [NotMapped]
    public bool? HasAttachment { get; set; }
    public ICollection<OperatorAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<OperatorAttachment>().ToList();
    }


    [MaxLength(256)]
    public string? NormalizedTitle { get; set; }
    [MaxLength(64)]
    public string? NormalizedIdentificationNumber { get; set; }
    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }
}