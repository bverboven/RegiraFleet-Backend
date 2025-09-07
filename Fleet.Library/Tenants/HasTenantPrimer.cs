using Microsoft.EntityFrameworkCore.ChangeTracking;
using Regira.Entities.EFcore.Primers.Abstractions;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Tenants;

public class HasTenantPrimer(ITenantContext tenantContext) : EntityPrimerBase<IHasTenantId>
{
    public override Task PrepareAsync(IHasTenantId entity, EntityEntry entry)
    {
        if (!string.IsNullOrWhiteSpace(tenantContext.TenantId))
        {
            entity.TenantId = tenantContext.TenantId;
        }

        return Task.CompletedTask;
    }
}