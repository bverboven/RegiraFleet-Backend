using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.Vehicles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Entities.Interventions;

public class Intervention : IFleetEntity, IEntityWithSerial, IHasDescription, IHasNormalizedContent, IHasAttachments, IHasAttachments<InterventionAttachment>
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    public int ClientId { get; set; }
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
    public int? Mileage { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public DateTime? InterventionDate { get; set; }


    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }


    public Vehicle? Vehicle { get; set; }
    public Operator? Operator { get; set; }
    public ICollection<Invoice>? Invoices { get; set; }
    public ICollection<InterventionInterventionType>? InterventionTypes { get; set; }

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