using Regira.Entities.Mapping.Models;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Models.Vehicles;

public class VehicleDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public string TenantId { get; set; } = null!;
    public int? BrandId { get; set; }
    public int? VehicleTypeId { get; set; }
    public string Code { get; set; } = null!;
    public string? Model { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }
    public Guid ConcurrencyToken { get; set; }

    public virtual BrandDto? Brand { get; set; }
    public virtual VehicleTypeDto? VehicleType { get; set; }
    public ICollection<VehicleInterventionTypeDto>? InterventionTypes { get; set; }
    public ICollection<EntityLabelDto>? Labels { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}