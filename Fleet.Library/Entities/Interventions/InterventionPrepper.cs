using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Preppers.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionPrepper(FleetContextBase dbContext) : EntityPrepperBase<Intervention>
{
    public override Task Prepare(Intervention modified, Intervention? original)
    {
        if (modified.Invoice != null)
        {
            modified.Invoice.InterventionId = modified.Id;
        }

        if (original != null)
        {
            if (modified.Invoice != null)
            {
                original.Invoice = modified.Invoice;
                dbContext.Entry(modified.Invoice).State = original.Invoice.Id > 0 
                    ? EntityState.Modified 
                    : EntityState.Added;
            }

            if (original.Invoice != null && modified.Invoice == null)
            {
                dbContext.Entry(original.Invoice).State = EntityState.Deleted;
            }
        }

        return Task.CompletedTask;
    }
}