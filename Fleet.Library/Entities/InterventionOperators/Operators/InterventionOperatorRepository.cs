using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class InterventionOperatorRepository(FleetContext dbContext) : FleetRepositoryBase<InterventionOperator, InterventionOperatorSearchObject, EntitySortBy, InterventionOperatorIncludes>(dbContext)
{
    public override IQueryable<InterventionOperator> Filter(IQueryable<InterventionOperator> query, InterventionOperatorSearchObject? so)
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

    public override IQueryable<InterventionOperator> AddIncludes(IQueryable<InterventionOperator> query, InterventionOperatorIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(InterventionOperatorIncludes.ContactData))
            {
                query = query
                    .Include(x => x.ContactData);
            }
            if (includes.Value.HasFlag(InterventionOperatorIncludes.Addresses))
            {
                query = query
                    .Include(x => x.Addresses);
            }
            if (includes.Value.HasFlag(InterventionOperatorIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes);
            }
            if (includes.Value.HasFlag(InterventionOperatorIncludes.Attachments))
            {
                query = query
                    .Include(x => x.Attachments);
            }
        }

        return query;
    }
}