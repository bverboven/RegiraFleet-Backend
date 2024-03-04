using Regira.Entities.Web.Attachments.Models;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Vehicles;

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
    public bool IsArchived { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}