using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Entities.Clients.Subscriptions;
using Regira.Fleet.Identity.Entities.Users.Claims;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Entities.Clients;

public class Client : IEntity<string>, IHasCode, IHasNormalizedTitle, IHasTimestamps
{
    [StringLength(32)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    [MaxLength(8)]
    public string? DefaultCulture { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }

    [MaxLength(256)]
    [Normalized(SourceProperties = [nameof(Title), nameof(Code)])]
    public string? NormalizedTitle { get; set; }

    public ICollection<ClientLanguage>? Languages { get; set; }
    public ICollection<ClientSubscription>? Subscriptions { get; set; }
    public ICollection<ClientUserClaim>? UserClaims { get; set; }
}
