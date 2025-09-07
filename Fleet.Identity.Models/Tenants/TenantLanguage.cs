using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Identity.Models.Tenants;

public class TenantLanguage : IEntity, IHasTenantId
{
    [StringLength(32)]
    public string TenantId { get; set; } = null!;
    [StringLength(2)]
    public string LangCode { get; set; } = null!;
}