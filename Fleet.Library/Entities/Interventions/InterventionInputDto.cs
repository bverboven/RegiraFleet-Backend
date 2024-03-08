using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Interventions.Invoices;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionInputDto
{
    public int Id { get; set; }
    [Required]
    public int VehicleId { get; set; }
    [Required]
    public int OperatorId { get; set; }
    public int? Mileage { get; set; }

    public string? Description { get; set; }

    public DateTime? InterventionDate { get; set; }

    public ICollection<InvoiceInputDto>? Invoices { get; set; }
    public ICollection<InterventionInterventionTypeInputDto>? InterventionTypes { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}