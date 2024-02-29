using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Cars.Brands;
using Regira.Fleet.Entities.Cars.CarTypes;

namespace Regira.Fleet.Entities.Cars;

public class CarDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public int ClientId { get; set; }
    public int? BrandId { get; set; }
    public int? CarTypeId { get; set; }
    public string? Code { get; set; }
    public string? Model { get; set; }
    public string? Notes { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public virtual BrandDto? Brand { get; set; }
    public virtual CarTypeDto? CarType { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}