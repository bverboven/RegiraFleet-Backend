using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Clients.Users;

public class UserAction : IEntityWithSerial, IHasUserId, IHasTitle, IHasCreated
{
    public int Id { get; set; }
    [StringLength(32)]
    public string Client { get; set; } = null!;
    [MaxLength(255)]
    public string UserId { get; set; } = null!;
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    public int EntityId { get; set; }
    [MaxLength(64)]
    public string? EntityType { get; set; }
    public string? Url { get; set; }
    public string? Body { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}
