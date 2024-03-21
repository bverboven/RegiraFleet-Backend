using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Entities.Clients;

public class ClientLanguage : IEntity
{
    public string ClientId { get; set; } = null!;
    [StringLength(2)]
    public string LangCode { get; set; } = null!;
}