using System.ComponentModel.DataAnnotations;
using Regira.Entities.Web.Attachments.Models;

namespace Regira.Fleet.Entities.Cars;

public class CarInputDto
{
    public int? BrandId { get; set; }
    public int? CarTypeId { get; set; }
    [Required]
    [MaxLength(8)]
    public string Code { get; set; } = null!;
    [MaxLength(64)]
    public string? Model { get; set; }
    public bool IsArchived { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}