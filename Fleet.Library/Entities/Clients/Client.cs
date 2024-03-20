using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Entities.Clients.Subscriptions;
using Regira.Fleet.Entities.Clients.Users;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Clients;

public class Client : IEntityWithSerial, IHasTitle
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    [MaxLength(8)]
    public string? DefaultCulture { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }

    [MaxLength(256)]
    [Normalized(SourceProperties = new[] { nameof(Title), nameof(Code) })]
    public string? NormalizedTitle { get; set; }


    public ICollection<ClientLanguage>? Languages { get; set; }
    public ICollection<ClientSubscription>? Subscriptions { get; set; }


    public ICollection<ClientUserClaim>? UserClaims { get; set; }
    public ICollection<Brand>? Brands { get; set; }
    public ICollection<Intervention>? Interventions { get; set; }
    public ICollection<InterventionType>? InterventionTypes { get; set; }
    public ICollection<Operator>? InterventionOperators { get; set; }
    public ICollection<Vehicle>? Vehicles { get; set; }
    public ICollection<VehicleType>? VehicleTypes { get; set; }
}
