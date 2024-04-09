using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Models.Abstractions;
using Regira.Fleet.Models.Interventions.Invoices;
using Regira.Fleet.Models.InterventionTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Models.Interventions.Action;

public class InterventionAction : IFleetEntity, IEntityWithSerial, IHasDescription, IHasNormalizedContent, IHasAttachments, IHasAttachments<InterventionActionAttachment>
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    [StringLength(32)]
    public string ClientId { get; set; } = null!;
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
    public int InvoiceId { get; set; }
    public int? Mileage { get; set; }
    public string? Description { get; set; }


    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }

    public Invoice? Invoice { get; set; }
    public InterventionType? InterventionType { get; set; }

    [NotMapped]
    public bool? HasAttachment { get; set; }
    public ICollection<InterventionActionAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<InterventionActionAttachment>().ToList();
    }


    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }
}
