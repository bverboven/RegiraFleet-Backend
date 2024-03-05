using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Regira.Fleet.Entities.Clients;

public class Client : IEntityWithSerial, IHasTitle
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    [NotMapped]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }


    public ICollection<ClientUserClaim>? UserClaims { get; set; }
    public ICollection<Brand>? Brands { get; set; }
    public ICollection<Intervention>? Interventions { get; set; }
    public ICollection<InterventionType>? InterventionTypes { get; set; }
    public ICollection<Operator>? InterventionOperators { get; set; }
    public ICollection<Vehicle>? Vehicles { get; set; }
    public ICollection<VehicleType>? VehicleTypes { get; set; }
}