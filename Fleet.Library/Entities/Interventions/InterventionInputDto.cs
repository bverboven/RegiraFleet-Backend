using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Interventions.Invoices;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionInputDto
{
    [Required]
    public int VehicleId { get; set; }
    [Required]
    public int SupplierId { get; set; }

    public int? Mileage { get; set; }
    [MaxLength(256)]
    public string? Comments { get; set; }
    public string? Notes { get; set; }

    public InvoiceInputDto? Invoice { get; set; }
    public ICollection<InterventionTypeInputDto>? InterventionTypes { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}