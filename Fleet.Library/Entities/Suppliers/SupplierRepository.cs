using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Suppliers;

public class SupplierRepository(FleetContext dbContext) : FleetRepository<Supplier, SupplierSearchObject, EntitySortBy, SupplierIncludes>(dbContext)
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

    public override IQueryable<Supplier> AddIncludes(IQueryable<Supplier> query, SupplierIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(SupplierIncludes.ContactData))
            {
                query = query
                    .Include(x => x.ContactData);
            }
            if (includes.Value.HasFlag(SupplierIncludes.Addresses))
            {
                query = query
                    .Include(x => x.Addresses);
            }
            if (includes.Value.HasFlag(SupplierIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes);
            }
            if (includes.Value.HasFlag(SupplierIncludes.Attachments))
            {
                query = query
                    .Include(x => x.Attachments);
            }
        }

        return query;
    }
}