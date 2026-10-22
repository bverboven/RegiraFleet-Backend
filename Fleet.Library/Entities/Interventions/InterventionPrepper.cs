using Microsoft.EntityFrameworkCore;
using Regira.Entities.Preppers.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.Interventions.Invoices;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionPrepper(FleetContextBase dbContext) : EntityPrepperBase<Intervention>
{
    public override async Task Prepare(Intervention modified, Intervention? original, CancellationToken cancellationToken = default)
    {
        if (modified.Invoice != null)
        {
            modified.Invoice.InterventionId = modified.Id;
            // an intervention has one invoice, its own: its id is the stored one, never the client's,
            // or a save could take over another intervention's invoice, another tenant's included
            modified.Invoice.Id = modified.Id == 0
                ? 0
                : await dbContext.Set<Invoice>()
                    .Where(x => x.InterventionId == modified.Id)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);
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
    }
}