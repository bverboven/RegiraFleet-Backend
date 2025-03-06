using System.ComponentModel.DataAnnotations;
using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.Interventions.Invoices;

namespace Regira.Fleet.Models.Interventions;

public class InterventionInputDto
{
    public int Id { get; set; }
    [Required]
    public int? VehicleId { get; set; }
    [Required]
    public int? OperatorId { get; set; }
    public int? InterventionTypeId { get; set; }
    public DateTime? InterventionDate { get; set; }
    public int? Mileage { get; set; }
    public string? Description { get; set; }


    public InvoiceInputDto? Invoice { get; set; }
    public ICollection<EntityLabelInputDto>? Labels { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}