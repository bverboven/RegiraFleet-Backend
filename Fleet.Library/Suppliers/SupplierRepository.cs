using Regira.Entities.EFcore.Extensions;
using Regira.Fleet.DAL.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Suppliers;

public class SupplierRepository(FleetContext dbContext) : DefaultFleetRepository<Supplier, SupplierSearchObject>(dbContext)
{
    public override IQueryable<Supplier> Filter(IQueryable<Supplier> query, SupplierSearchObject? so)
    {
        query = base.Filter(query, so);
        if (so != null)
        {
            query = query.FilterArchivable(so.IsArchived);
            query = query.FilterCode(so.Code);

            if (!string.IsNullOrWhiteSpace(so.Name))
            {
                query = query.Where(x => x.Name!.StartsWith(so.Name, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(so.VATNumber))
            {
                query = query.Where(x => x.VATNumber!.Equals(so.VATNumber, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(so.Phone))
            {
                query = query.Where(x => x.Phone1 == so.Phone || x.Phone2 == so.Phone);
            }
        }

        return query;
    }
}