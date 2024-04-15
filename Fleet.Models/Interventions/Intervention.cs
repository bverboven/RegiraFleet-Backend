using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Models.Abstractions;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions.Actions;
using Regira.Fleet.Models.Interventions.Invoices;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Models.Interventions;

public class Intervention : IFleetEntity, IEntityWithSerial, IHasDescription, IHasNormalizedContent, 
    IHasLabels<InterventionLabel>, IHasLabels, IHasAttachments, IHasAttachments<InterventionAttachment>
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    [StringLength(32)]
    public string ClientId { get; set; } = null!;
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
    public int? InterventionTypeId { get; set; }
    public DateTime? InterventionDate { get; set; }
    public int? Mileage { get; set; }
    public string? Description { get; set; }


    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }


    public Vehicle? Vehicle { get; set; }
    public Operator? Operator { get; set; }
    public Invoice? Invoice { get; set; }
    public InterventionType? InterventionType { get; set; }
    public ICollection<InterventionAction>? Actions { get; set; }

    // Labels
    public ICollection<InterventionLabel>? Labels { get; set; }
    ICollection<IEntityLabel>? IHasLabels.Labels
    {
        get => Labels?.Cast<IEntityLabel>().ToList();
        set => Labels = value?.Cast<InterventionLabel>().ToList();
    }

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