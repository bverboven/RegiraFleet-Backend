using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Entities.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Utilities;
using System.Security.Claims;

namespace Regira.Fleet.Identity.Entities.Users;
internal class FleetUserRepository(AccountsContext dbContext, UserManager<FleetUser> userManager) : IEntityRepository<FleetUser, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes>
{
    public async Task<FleetUser?> Details(string id)
    {
        var item = await dbContext.Users
            .Include(x => x.UserClaims)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.Id == id);
        return item;
    }
    public async Task<IList<FleetUser>> List(IList<FleetUserSearchObject?> searchObjects, IList<EntitySortBy> sortBy, FleetUserIncludes? includes = null, PagingInfo? pagingInfo = null)
    {
        IQueryable<FleetUser> query = Query(dbContext.Users, searchObjects, includes, pagingInfo);
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
    public IQueryable<FleetUser> AddIncludes(IQueryable<FleetUser> query, FleetUserIncludes? includes)
    {
        if (includes != null)
        {
            if (includes.Value.HasFlag(FleetUserIncludes.UserClaims))
            {
                query = query.Include(x => x.UserClaims);
            }
            if (includes.Value.HasFlag(FleetUserIncludes.ClientClaims))
            {
                query = query.Include(x => x.ClientClaims);
            }
        }

        return query;
    }
    public virtual IQueryable<FleetUser> Query(IQueryable<FleetUser> query, IList<FleetUserSearchObject?> searchObjects, FleetUserIncludes? includes, PagingInfo? pagingInfo)
    {
        var filteredQuery = Filter(query, searchObjects);
        var sortedQuery = filteredQuery.OrderBy(x => x.UserName);
        var pagedQuery = sortedQuery.PageQuery(pagingInfo);
        var includingQuery = AddIncludes(pagedQuery, includes);

        return includingQuery;
    }


    public async Task Add(FleetUser item)
    {
        var result = string.IsNullOrWhiteSpace(item.NewPassword)
            ? await userManager.CreateAsync(item)
            : await userManager.CreateAsync(item, item.NewPassword);
        if (result.Succeeded)
        {
            await Modify(item, item);
        }
    }
    public async Task Modify(FleetUser item)
    {
        var original = await Details(item.Id);
        if (original != null)
        {
            await Modify(item, original);
        }
    }
    public async Task Save(FleetUser item)
    {
        var original = await Details(item.Id);
        if (original != null)
        {
            await Modify(item, original);
        }
        else
        {
            await Add(item);
        }
    }
    public Task Remove(FleetUser item)
        => userManager.DeleteAsync(item);

    public async Task Modify(FleetUser item, FleetUser original)
    {
        await userManager.UpdateAsync(item);

        if (item.UserClaims != null)
        {
            var originalClaims = await userManager.GetClaimsAsync(original);
            var claimsToRemove = originalClaims.Where(oc => item.UserClaims.All(c => c.ClaimType != oc.Type && c.ClaimValue != oc.Value));
            var claimsToAdd = item.UserClaims.Where(c => originalClaims.All(oc => c.ClaimType != oc.Type && c.ClaimValue != oc.Value));

            if (claimsToRemove.Any())
            {
                await userManager.RemoveClaimsAsync(original, claimsToRemove);
            }
            if (claimsToAdd.Any())
            {
                await userManager.AddClaimsAsync(original, claimsToAdd.Select(c => new Claim(c.ClaimType!, c.ClaimValue!)));
            }
        }

        if (item.ClientClaims != null && original.ClientClaims != null)
        {
            var originalClaims = original.ClientClaims;
            var claimsToRemove = originalClaims.Where(oc => item.ClientClaims.All(c => c.ClientId != oc.ClientId && c.ClaimType != oc.ClaimType && c.ClaimValue != oc.ClaimValue));
            var claimsToAdd = item.ClientClaims.Where(c => originalClaims.All(oc => c.ClientId != oc.ClientId && c.ClaimType != oc.ClaimType && c.ClaimValue != oc.ClaimValue));

            if (claimsToRemove.Any())
            {
                dbContext.ClientUserClaims.RemoveRange(originalClaims);
            }
            if (claimsToAdd.Any())
            {
                dbContext.ClientUserClaims.AddRange(claimsToAdd);
            }
            await SaveChanges();
        }
    }


    public Task<int> SaveChanges(CancellationToken token = default)
        => dbContext.SaveChangesAsync(token);


    protected FleetUserSearchObject? Convert(object? so)
        => so == default ? default
            : so is FleetUserSearchObject tso ? tso
            : ObjectUtility.Create<FleetUserSearchObject>(so);

}
