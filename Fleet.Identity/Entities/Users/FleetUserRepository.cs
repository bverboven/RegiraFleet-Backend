using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Entities.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Utilities;

namespace Regira.Fleet.Identity.Entities.Users;
internal class FleetUserRepository(AccountsContext dbContext) : IEntityRepository<FleetUser, string, FleetUserSearchObject, EntitySortBy, EntityIncludes>
{
    public async Task<FleetUser?> Details(string id)
    {
        var item = await dbContext.Users
            .Include(x => x.UserClaims)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.Id == id);
        return item;
    }
    public async Task<IList<FleetUser>> List(IList<FleetUserSearchObject?> searchObjects, IList<EntitySortBy> sortBy, EntityIncludes? includes = null, PagingInfo? pagingInfo = null)
    {
        IQueryable<FleetUser> query = Query(dbContext.Users, searchObjects, pagingInfo);
        var items = await query
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();
        return items;
    }
    public Task<IList<FleetUser>> List(object? so = null, PagingInfo? pagingInfo = null)
        => List(Convert(so), pagingInfo);
    public Task<int> Count(IList<FleetUserSearchObject?> searchObjects)
    {
        var query = Filter(dbContext.Users, searchObjects.Select(Convert).ToList());
        return query.CountAsync();
    }
    public Task<int> Count(object? so)
        => Count(Convert(so));

    public IQueryable<FleetUser> Filter(IQueryable<FleetUser> query, FleetUserSearchObject? so)
    {
        if (so != null)
        {
            if (!string.IsNullOrWhiteSpace(so.ClientId))
            {
                query = query.Where(x => x.UserClaims!.Any(c => c.ClaimType == FleetClaimTypes.ClientId && c.ClaimValue == so.ClientId));
            }
        }
        return query;
    }
    public IQueryable<FleetUser> Filter(IQueryable<FleetUser> query, IList<FleetUserSearchObject?> searchObjects)
        => searchObjects.Aggregate((IQueryable<FleetUser>?)null, (r, so) => r == null ? Filter(query, so) : r.Union(Filter(query, so))) ?? query;
    public virtual IQueryable<FleetUser> Query(IQueryable<FleetUser> query, IList<FleetUserSearchObject?> searchObjects, PagingInfo? pagingInfo)
    {
        var filteredQuery = Filter(query, searchObjects);
        var sortedQuery = filteredQuery.OrderBy(x => x.UserName);
        var pagedQuery = sortedQuery.PageQuery(pagingInfo);
        var includingQuery = pagedQuery;

        return includingQuery;
    }


    public Task Add(FleetUser item)
    {
        throw new NotImplementedException();
    }
    public Task Modify(FleetUser item)
    {
        throw new NotImplementedException();
    }
    public Task Save(FleetUser item)
    {
        throw new NotImplementedException();
    }
    public Task Remove(FleetUser item)
    {
        throw new NotImplementedException();
    }


    public Task<int> SaveChanges(CancellationToken token = default)
        => dbContext.SaveChangesAsync(token);


    protected FleetUserSearchObject? Convert(object? so)
        => so == default ? default
            : so is FleetUserSearchObject tso ? tso
            : ObjectUtility.Create<FleetUserSearchObject>(so);

}
