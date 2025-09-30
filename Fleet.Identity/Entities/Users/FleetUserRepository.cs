using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Entities.Services.Abstractions;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Models.Users.Claims;
using Regira.Utilities;

namespace Regira.Fleet.Identity.Entities.Users;
public class FleetUserRepository(AccountsContextBase dbContext, UserManager<FleetUser> userManager, IEnumerable<IFilteredQueryBuilder<FleetUser, string, FleetUserSearchObject>> queryFilters, IMapper mapper)
    : IEntityRepository<FleetUserModel, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes>
{
    protected AccountsContextBase DbContext => dbContext;

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
    public Task<IList<FleetUserModel>> List(FleetUserSearchObject? so = null, PagingInfo? pagingInfo = null)
        => List([so], [], null, pagingInfo);


    public Task<IList<FleetUserModel>> List(object? so = null, PagingInfo? pagingInfo = null)
        => List([Convert(so)], [], null, pagingInfo);
    public Task<long> Count(IList<FleetUserSearchObject?> searchObjects)
    {
        var query = Filter(dbContext.Users, searchObjects.Select(Convert).ToList());
        return query.LongCountAsync();
    }
    public Task<long> Count(object? so)
        => Count([Convert(so)]);
    public Task<long> Count(FleetUserSearchObject? so)
        => Count([so]);

    public Task<FleetUser?> GetItem(string id)
    {
        return AddIncludes(dbContext.Users, FleetUserIncludes.All)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public IQueryable<FleetUser> Filter(IQueryable<FleetUser> query, FleetUserSearchObject? so)
    {
        foreach (var filter in queryFilters)
        {
            query = filter.Build(query, so);
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
            if (includes.Value.HasFlag(FleetUserIncludes.TenantClaims))
            {
                query = query.Include(x => x.TenantClaims!)
                    .ThenInclude(x => x.Tenant);
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
    public async Task<FleetUserModel?> Modify(FleetUserModel model)
    {
        var original = await GetItem(model.Id);
        if (original != null)
        {
            PrepareItem(model, original);
            await Modify(model, original);
            await UpdateUser(model, original);

            return model;
        }

        return null;
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
        if (string.IsNullOrWhiteSpace(model.Id))
        {
            model.Id = Guid.NewGuid().ToString();
        }

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
        if (model.TenantClaims?.Any() == true)
        {
            foreach (var claim in model.TenantClaims)
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
            throw new Exception(result.Errors.FirstOrDefault()?.Code);
        }
    }
    public Task Modify(FleetUserModel item, FleetUser original)
    {
        if (item.UserClaims != null)
        {
            var originalClaims = original.UserClaims!;
            var claimsToRemove = originalClaims
                .Where(oc => item.UserClaims.All(c => c.Id != oc.Id))
                .ToArray();
            var claimsToAdd = item.UserClaims
                .Where(c => originalClaims.All(oc => c.Id != oc.Id))
                .ToArray();
            var claimsToUpdate = originalClaims.Except(claimsToRemove)
                .ToArray();

            if (claimsToRemove.Any())
            {
                dbContext.UserClaims.RemoveRange(claimsToRemove);
            }
            if (claimsToAdd.Any())
            {
                dbContext.UserClaims.AddRange(claimsToAdd);
            }
            if (claimsToUpdate.Any())
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
        dbContext.UpdateEntityChildCollection<FleetUserModel, TenantUserClaim, int>(originalModel, item, model => model.TenantClaims, (model, collection) => model.TenantClaims = collection);
        return Task.CompletedTask;
    }


    public Task<int> SaveChanges(CancellationToken token = default)
        => dbContext.SaveChangesAsync(token);


    protected FleetUserSearchObject? Convert(object? so)
        => so == null ? null
            : so as FleetUserSearchObject ?? ObjectUtility.Create<FleetUserSearchObject>(so);
}
