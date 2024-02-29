using System.ComponentModel.DataAnnotations;
using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Interventions.Invoices;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionInputDto
{
    [Required]
    public int CarId { get; set; }
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public int InterventionTypeId { get; set; }

    public int? Mileage { get; set; }
    [MaxLength(256)]
    public string? Comments { get; set; }
    public string? Notes { get; set; }

    public InvoiceInputDto? Invoice { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}