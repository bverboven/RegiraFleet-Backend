using System.ComponentModel.DataAnnotations;
using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Models.Vehicles;

public class VehicleInputDto
{
    public int Id { get; set; }
    public int? BrandId { get; set; }
    public int? VehicleTypeId { get; set; }
    [Required]
    [MaxLength(8)]
    public string Code { get; set; } = null!;
    [MaxLength(64)]
    public string? Model { get; set; }
    [MaxLength(64)]
    public string? IdentificationNumber { get; set; }
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
    public ICollection<InterventionTypeInputDto>? InterventionTypes { get; set; }
    public ICollection<EntityLabelInputDto>? Labels { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}