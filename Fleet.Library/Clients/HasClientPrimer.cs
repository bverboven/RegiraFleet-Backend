using Microsoft.EntityFrameworkCore.ChangeTracking;
using Regira.Entities.EFcore.Primers.Abstractions;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Clients;

public class HasClientPrimer(IClientContext clientContext) : EntityPrimerBase<IHasClientId>
{
    public override Task PrepareAsync(IHasClientId entity, EntityEntry entry)
    {
        if (!string.IsNullOrWhiteSpace(clientContext.ClientId))
        {
            entity.ClientId = clientContext.ClientId;
        }

        return Task.CompletedTask;
    }
}