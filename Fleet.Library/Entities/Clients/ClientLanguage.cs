using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Clients;

public class ClientLanguage : IEntity
{
    public int ClientId { get; set; }
    [StringLength(2)]
    public string LangCode { get; set; } = null!;
}