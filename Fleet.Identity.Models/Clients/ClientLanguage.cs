using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Identity.Models.Clients;

public class ClientLanguage : IEntity
{
    [StringLength(32)]
    public string ClientId { get; set; } = null!;
    [StringLength(2)]
    public string LangCode { get; set; } = null!;
}