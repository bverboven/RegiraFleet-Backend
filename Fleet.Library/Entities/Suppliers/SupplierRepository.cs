using Regira.Entities.EFcore.Extensions;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Suppliers;

public class SupplierRepository(FleetContext dbContext) : FleetRepository<Supplier, SupplierSearchObject>(dbContext)
{
    public override IQueryable<Supplier> Filter(IQueryable<Supplier> query, SupplierSearchObject? so)
    {
        query = base.Filter(query, so);
        if (so != null)
        {
            query = query.FilterArchivable(so.IsArchived);
            query = query.FilterCode(so.Code);

            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                query = query.Where(x => x.Title!.StartsWith(so.Title, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(so.IdentificationNumber))
            {
                query = query.Where(x => x.IdentificationNumber!.Equals(so.IdentificationNumber, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(so.Phone))
            {
                query = query.Where(x => x.ContactData!.Any(cd => cd.Value == so.Phone));
            }
        }

        return query;
    }
}