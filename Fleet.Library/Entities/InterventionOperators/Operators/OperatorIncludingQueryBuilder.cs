using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorIncludingQueryBuilder : IIncludableQueryBuilder<Operator, int, OperatorIncludes>
{
    public IQueryable<Operator> AddIncludes(IQueryable<Operator> query, OperatorIncludes? includes = null)
    {
        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(OperatorIncludes.ContactData))
            {
                query = query.Include(x => x.ContactData!.OrderBy(a => a.SortOrder));
            }
            if (includes.Value.HasFlag(OperatorIncludes.Addresses))
            {
                query = query.Include(x => x.Addresses!.OrderBy(a => a.SortOrder));
            }
            if (includes.Value.HasFlag(OperatorIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes!)
                    .ThenInclude(x => x.InterventionType);
            }
            // Labels
            if (includes.Value.HasFlag(OperatorIncludes.Labels))
            {
                query = query.Include(x => x.Labels!.OrderBy(a => a.SortOrder));
            }
            // Attachments
            if (includes.Value.HasFlag(OperatorIncludes.Attachments))
            {
                query = query
                    .Include(x => x.Attachments!.OrderBy(a => a.SortOrder))
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }
}