using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Entities.Users.Claims;
using Regira.Utilities;

namespace Regira.Fleet.Identity.Entities.Users;
internal class FleetUserRepository(AccountsContext dbContext, UserManager<FleetUser> userManager, IMapper mapper) : IEntityRepository<FleetUserModel, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes>
{
    public async Task<FleetUserModel?> Details(string id)
    {
        var item = await GetItem(id);
        return mapper.Map<FleetUserModel>(item);
    }
    public async Task<IList<FleetUserModel>> List(IList<FleetUserSearchObject?> searchObjects, IList<EntitySortBy> sortBy, FleetUserIncludes? includes = null, PagingInfo? pagingInfo = null)
    {
        IQueryable<FleetUser> query = Query(dbContext.Users, searchObjects, includes, pagingInfo);
        var items = await query
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();
        return mapper.Map<List<FleetUserModel>>(items);
    }
    public Task<IList<FleetUserModel>> List(object? so = null, PagingInfo? pagingInfo = null)
        => List(Convert(so), pagingInfo);
    public Task<int> Count(IList<FleetUserSearchObject?> searchObjects)
    {
        var query = Filter(dbContext.Users, searchObjects.Select(Convert).ToList());
        return query.CountAsync();
    }
    public Task<int> Count(object? so)
        => Count(new[] { Convert(so) });

    public Task<FleetUser?> GetItem(string id)
    {
        return AddIncludes(dbContext.Users, FleetUserIncludes.All)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public IQueryable<FleetUser> Filter(IQueryable<FleetUser> query, FleetUserSearchObject? so)
    {
        if (so != null)
        {
            query = query.FilterId(so.Id);
            //if (!string.IsNullOrWhiteSpace(so.Id))
            //{
            //    query = query.Where(x => x.Id == so.Id);
            //}
            query = query.FilterIds(so.Ids);
            if (!string.IsNullOrWhiteSpace(so.ClientId))
            {
                query = query.Where(x => x.ClientClaims!.Any(c => c.ClientId == so.ClientId));
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
                query = query.Include(x => x.ClientClaims!)
                    .ThenInclude(x => x.Client);
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


    public async Task Add(FleetUserModel model)
    {
        var item = mapper.Map<FleetUser>(model);
        var result = string.IsNullOrWhiteSpace(item.NewPassword)
            ? await userManager.CreateAsync(item)
            : await userManager.CreateAsync(item, item.NewPassword);
        if (result.Succeeded)
        {
            PrepareItem(model, null);
            await Modify(model, item);
        }
    }
    public async Task Modify(FleetUserModel model)
    {
        var original = await GetItem(model.Id);
        if (original != null)
        {
            PrepareItem(model, original);
            await Modify(model, original);
            await UpdateAndCleanUp(original);
        }
    }
    public async Task Save(FleetUserModel model)
    {
        var original = await GetItem(model.Id);
        if (original != null)
        {
            PrepareItem(model, original);
            await Modify(model, original);
            await UpdateAndCleanUp(original);
        }
        else
        {
            await Add(model);
        }
    }
    public Task Remove(FleetUserModel item)
        => userManager.DeleteAsync(mapper.Map<FleetUser>(item));

    public void PrepareItem(FleetUserModel item, FleetUser? original)
    {
        item.Email ??= original?.Email!;
        if (item.UserClaims?.Any() == true)
        {
            foreach (var claim in item.UserClaims)
            {
                claim.UserId = item.Id;
            }
        }
        if (item.ClientClaims?.Any() == true)
        {
            foreach (var claim in item.ClientClaims)
            {
                claim.UserId = item.Id;
            }
        }
    }
    public async Task UpdateAndCleanUp(FleetUser item)
    {
        dbContext.Entry(item).State = EntityState.Modified;
        var result = await userManager.UpdateAsync(item);
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors?.FirstOrDefault()?.Code);
        }
    }
    public async Task Modify(FleetUserModel item, FleetUser original)
    {
        dbContext.Entry(original).CurrentValues.SetValues(item);

        if (item.UserClaims != null)
        {
            var originalClaims = original.UserClaims!;
            var claimsToRemove = originalClaims.Where(oc => item.UserClaims.All(c => c.Id != oc.Id));
            var claimsToAdd = item.UserClaims.Where(c => originalClaims.All(oc => c.Id != oc.Id));
            var claimsToUpdate = originalClaims.Except(claimsToRemove);

            if (claimsToRemove.Any())
            {
                dbContext.UserClaims.RemoveRange(claimsToRemove);
            }
            if (claimsToAdd.Any())
            {
                dbContext.AddRange(claimsToAdd);
            }
            if (claimsToUpdate?.Any() == true)
            {
                foreach (var claim in claimsToUpdate)
                {
                    var itemClaim = item.UserClaims.First(c => c.Id == claim.Id);
                    if (itemClaim.ClaimValue != claim.ClaimValue)
                    {
                        claim.ClaimValue = itemClaim.ClaimValue;
                        dbContext.Entry(claim).State = EntityState.Modified;
                    }
                }
            }
        }

        var originalModel = mapper.Map<FleetUserModel>(original);
        dbContext.UpdateEntityChildCollection<FleetUserModel, string, ClientUserClaim, int>(originalModel, item, item => item.ClientClaims, (item, collection) => item.ClientClaims = collection);
    }


    public Task<int> SaveChanges(CancellationToken token = default)
        => dbContext.SaveChangesAsync(token);


    protected FleetUserSearchObject? Convert(object? so)
        => so == default ? default
            : so is FleetUserSearchObject tso ? tso
            : ObjectUtility.Create<FleetUserSearchObject>(so);
}
public static class Extensions
{
    public static void UpdateEntityChildCollection<TEntity, TEntityKey, TChild, TChildKey>(this DbContext dbContext, TEntity original, TEntity modified, Func<TEntity, ICollection<TChild>?> childrenGetter, Action<TEntity, ICollection<TChild>> childrenSetter, Action<TChild?, TChild>? processExtra = null)
    where TChild : class, IEntity<TChildKey>
    {
        var originalChildCollection = childrenGetter(original);
        var modifiedChildCollection = childrenGetter(modified);
        // ignore when no child collection is passed for either original OR modified entity
        if (originalChildCollection == null || modifiedChildCollection == null)
        {
            return;
        }

        var childrenToRemove = originalChildCollection!.Where(oc => modifiedChildCollection.All(c => !oc.Id!.Equals(c.Id)));
        var childrenToAdd = modifiedChildCollection!.Where(c => originalChildCollection.All(oc => !oc.Id!.Equals(c.Id)));
        var childrenToUpdate = originalChildCollection.Except(childrenToRemove);

        if (childrenToRemove.Any())
        {
            dbContext.RemoveRange(childrenToRemove);
        }
        if (childrenToAdd.Any())
        {
            foreach (var child in childrenToAdd)
            {
                processExtra?.Invoke(null, child);
                dbContext.Add(child);
            }
        }
        if (childrenToUpdate?.Any() == true)
        {
            foreach (var originalChild in childrenToUpdate)
            {
                var modifiedChild = modifiedChildCollection.First(c => c.Id!.Equals(originalChild.Id));
                processExtra?.Invoke(originalChild, modifiedChild);
                var childEntry = dbContext.Entry(originalChild);
                childEntry.CurrentValues.SetValues(modifiedChild);
                childEntry.State = EntityState.Modified;
            }
        }

        childrenSetter(original, modifiedChildCollection);
    }
}