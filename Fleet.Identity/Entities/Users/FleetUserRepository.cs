using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Models.Users.Claims;
using Regira.Fleet.Identity.Services;
using Regira.Utilities;

namespace Regira.Fleet.Identity.Entities.Users;
public class FleetUserRepository(AccountsContextBase dbContext, UserManager<FleetUser> userManager, IMapper mapper) : IEntityRepository<FleetUserModel, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes>
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
            var normalizer = new IdentityNormalizer();
            var qHelper = QKeywordHelper.Create(normalizer);

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

            if (!string.IsNullOrWhiteSpace(so.UserName))
            {
                var username = normalizer.Normalize(so.UserName);
                query = query.Where(x => x.NormalizedUserName == username);
            }

            if (!string.IsNullOrWhiteSpace(so.Name))
            {
                var qNames = qHelper.Parse(so.Name);
                var nameClaims = new[] { JwtClaimTypes.FamilyName, JwtClaimTypes.GivenName };
                foreach (var q in qNames)
                {
                    query = query.Where(x => x.GivenName!.ToUpper().Contains(q.Normalized!) || x.LastName!.ToUpper().Contains(q.Normalized!));
                }
            }

            if (!string.IsNullOrWhiteSpace(so.Culture))
            {
                query = query.Where(x => x.Culture == so.Culture);
            }

            if (!string.IsNullOrWhiteSpace(so.Q))
            {
                var keywords = qHelper.Parse(so.Q);
                foreach (var q in keywords)
                {
                    // ToDo: why can't EF translate ILike in this repo?
                    //query = query.Where(x =>
                    //    dbContext.ILike(x.NormalizedUserName!, q.Keyword!) || dbContext.ILike(x.NormalizedEmail!, q.QW!)
                    //    || dbContext.ILike(x.GivenName!, q.Keyword!) || dbContext.ILike(x.LastName!, q.QW!)
                    //);
                    query = query.Where(x =>
                        x.NormalizedUserName!.Contains(q.Normalized!) || x.NormalizedEmail!.Contains(q.Normalized!)
                        || x.GivenName!.ToUpper().Contains(q.Normalized!) || x.LastName!.ToUpper().Contains(q.Normalized!)
                    );
                }
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
        PrepareItem(model, null);
        var item = mapper.Map<FleetUser>(model);
        var result = string.IsNullOrWhiteSpace(item.NewPassword)
            ? await userManager.CreateAsync(item)
            : await userManager.CreateAsync(item, item.NewPassword);
        if (result.Succeeded)
        {
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
            await UpdateUser(model, original);
        }
    }
    public async Task Save(FleetUserModel model)
    {
        var original = await GetItem(model.Id);
        if (original != null)
        {
            PrepareItem(model, original);
            await Modify(model, original);
            await UpdateUser(model, original);
        }
        else
        {
            await Add(model);
        }
    }
    public Task Remove(FleetUserModel item)
        => userManager.DeleteAsync(mapper.Map<FleetUser>(item));

    public void PrepareItem(FleetUserModel model, FleetUser? original)
    {
        model.Id ??= Guid.NewGuid().ToString();
        if (original != null)
        {
            dbContext.Entry(original).CurrentValues.SetValues(model);
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                original.PasswordHash = userManager.PasswordHasher.HashPassword(original, model.NewPassword);
            }
        }
        if (model.UserClaims?.Any() == true)
        {
            foreach (var claim in model.UserClaims)
            {
                claim.UserId = model.Id;
            }
        }
        if (model.ClientClaims?.Any() == true)
        {
            foreach (var claim in model.ClientClaims)
            {
                claim.UserId = model.Id;
            }
        }
    }
    public async Task UpdateUser(FleetUserModel model, FleetUser item)
    {
        var entry = dbContext.Entry(item);
        if (entry.State == EntityState.Detached)
        {
            entry.State = EntityState.Modified;
        }
        var result = await userManager.UpdateAsync(item);
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors?.FirstOrDefault()?.Code);
        }
    }
    public async Task Modify(FleetUserModel item, FleetUser original)
    {
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