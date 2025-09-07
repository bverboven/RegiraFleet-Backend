using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Fleet.Identity.Models.Users;

namespace Regira.Fleet.Identity.Entities.Users;

public class UserQueryFilter: FilteredQueryBuilderBase<FleetUser, string, FleetUserSearchObject>
{
    public override IQueryable<FleetUser> Build(IQueryable<FleetUser> query, FleetUserSearchObject? so)
    {
        if (so != null)
        {
            // ID
            query = query.FilterId(so.Id);
            query = query.FilterIds(so.Ids);
            // Tenant
            if (!string.IsNullOrWhiteSpace(so.TenantId))
            {
                query = query.Where(x => x.TenantClaims!.Any(c => c.TenantId == so.TenantId));
            }
            // Culture
            if (!string.IsNullOrWhiteSpace(so.Culture))
            {
                query = query.Where(x => x.Culture == so.Culture);
            }
        }

        return query;
    }
}