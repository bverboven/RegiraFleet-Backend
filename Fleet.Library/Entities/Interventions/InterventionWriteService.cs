using Microsoft.EntityFrameworkCore;
using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.Services;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionWriteService(FleetContextBase dbContext, IEntityReadService<Intervention, int> readService)
    : EntityWriteService<FleetContextBase, Intervention, int>(dbContext, readService)
{
    public override async Task<Intervention?> Modify(Intervention item)
    {
        if (item.Invoice != null)
        {
            item.Invoice.InterventionId = item.Id;
        }

        item.Labels?.Prepare();

        var original = await base.Modify(item);

        if (original != null)
        {
            if (item.Invoice != null)
            {
                original.Invoice = item.Invoice;
                DbContext.Entry(item.Invoice).State =
                    original.Invoice.Id > 0 ? EntityState.Modified : EntityState.Added;
            }

            if (original.Invoice != null && item.Invoice == null)
            {
                DbContext.Entry(original.Invoice).State = EntityState.Deleted;
            }

            DbContext.UpdateEntityChildCollection(original, item, x => x.Labels,
                (x, collection) => x.Labels = collection);

            if (item.Attachments != null)
            {
                DbContext.ModifyEntityAttachments(original, item);
            }
        }

        return original;
    }
}