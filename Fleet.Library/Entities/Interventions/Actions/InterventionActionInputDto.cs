using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.InterventionTypes;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Interventions.Actions;

public class InterventionActionInputDto
{
    public int Id { get; set; }
    [Required]
    public int VehicleId { get; set; }
    [Required]
    public int OperatorId { get; set; }

    public int? Mileage { get; set; }
    [MaxLength(256)]
    public string? Comments { get; set; }
    public string? Notes { get; set; }

    public ICollection<InvoiceInputDto>? Invoices { get; set; }
    public ICollection<InterventionTypeInputDto>? InterventionTypes { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}