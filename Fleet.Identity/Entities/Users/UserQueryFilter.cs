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
            // Client
            if (!string.IsNullOrWhiteSpace(so.ClientId))
            {
                query = query.Where(x => x.ClientClaims!.Any(c => c.ClientId == so.ClientId));
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