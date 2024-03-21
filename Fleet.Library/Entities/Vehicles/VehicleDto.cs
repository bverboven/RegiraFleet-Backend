using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public int? BrandId { get; set; }
    public int? VehicleTypeId { get; set; }
    public string Code { get; set; } = null!;
    public string? Model { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public virtual BrandDto? Brand { get; set; }
    public virtual VehicleTypeDto? VehicleType { get; set; }
    public ICollection<InterventionTypeDto>? InterventionTypes { get; set; }
    //public ICollection<InterventionDto>? Interventions { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}