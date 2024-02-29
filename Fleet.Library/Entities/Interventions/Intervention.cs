using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Entities.Abstractions;
using Regira.Fleet.Entities.Cars;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.Suppliers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Entities.Interventions;

public class Intervention : IFleetEntity, IEntityWithSerial, IHasDescription, IHasNormalizedContent, IHasAttachments, IHasAttachments<InterventionAttachment>
{
    public int Id { get; set; }
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    public int ClientId { get; set; }
    public int? CarId { get; set; }
    public int? SupplierId { get; set; }
    public int? InterventionTypeId { get; set; }
    public int? InvoiceId { get; set; }
    public int? Mileage { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public string? Notes { get; set; }


    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }


    public Car? Car { get; set; }
    public Supplier? Supplier { get; set; }
    public InterventionType? InterventionType { get; set; }
    public Invoice? Invoice { get; set; }

    [NotMapped]
    public bool? HasAttachment { get; set; }
    public ICollection<InterventionAttachment>? Attachments { get; set; }
    ICollection<IEntityAttachment>? IHasAttachments.Attachments
    {
        get => Attachments?.Cast<IEntityAttachment>().ToList();
        set => Attachments = value?.Cast<InterventionAttachment>().ToList();
    }


    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }
}