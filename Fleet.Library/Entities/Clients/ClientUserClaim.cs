using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Entities.Clients;

public class ClientUserClaim : IEntityWithSerial, IHasCreated
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string UserId { get; set; } = null!;
    public string ClaimType { get; set; } = null!;
    public string? ClaimValue { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}
