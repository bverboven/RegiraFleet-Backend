using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleQueryBuilder(FleetContextBase dbContext, IQKeywordHelper qHelper,
    IEnumerable<IGlobalFilteredQueryBuilder> globalFilters,
    IEnumerable<IFilteredQueryBuilder<Vehicle, VehicleSearchObject>>? filters = null)
    : QueryBuilder<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes>(globalFilters, filters)
{
    public override IQueryable<Vehicle> Filter(IQueryable<Vehicle> query, VehicleSearchObject? so)
    {
        if (so != null)
        {
            // Code
            if (!string.IsNullOrWhiteSpace(so.Code))
            {
                var code = so.Code.PadLeft(3, '0');
                query = query.Where(x => x.Code == code);
            }
            // Model
            if (!string.IsNullOrWhiteSpace(so.Model))
            {
                query = query.Where(x => x.Model!.Equals(so.Model, StringComparison.InvariantCultureIgnoreCase));
            }
            // BrandId
            if (so.BrandId?.Any() == true)
            {
                query = query.Where(x => so.BrandId.Contains(x.BrandId!.Value));
            }
            // Brand
            if (!string.IsNullOrWhiteSpace(so.Brand))
            {
                query = query.Where(x => EF.Functions.Like(x.Brand!.Code!, so.Brand) ||
                                         EF.Functions.Like(x.Brand!.Title!, so.Brand));
            }
            // VehicleTypeId
            if (so.VehicleTypeId?.Any() == true)
            {
                query = query.Where(x => so.VehicleTypeId.Contains(x.VehicleTypeId!.Value));
            }
            // VehicleType
            if (!string.IsNullOrWhiteSpace(so.VehicleType))
            {
                query = query.Where(x => EF.Functions.Like(x.VehicleType!.Code!, so.VehicleType) ||
                                         EF.Functions.Like(x.VehicleType!.Title!, so.VehicleType));
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var kw = qHelper.ParseKeyword(so.Title.ToUpper());
                query = query.Where(x => EF.Functions.Like(x.NormalizedTitle!, kw.Q!));
            }
            // HasIntervention
            if (so.HasIntervention.HasValue)
            {
                query = query.Where(x => dbContext.Interventions.Any(i => i.VehicleId == x.Id));
            }
            // Q
            query = query.FilterQ(qHelper.Parse(so.Q?.ToUpper()));
        }

        return query;
    }
    public override IQueryable<Vehicle> SortBy(IQueryable<Vehicle> query, IList<VehicleSearchObject?>? so, EntitySortBy? sortBy, VehicleIncludes? includes)
        => query.OrderBy(x => x.Code);
    public override IQueryable<Vehicle> AddIncludes(IQueryable<Vehicle> query, IList<VehicleSearchObject?>? so, IList<EntitySortBy>? sortByList, VehicleIncludes? includes)
    {
        if (includes.HasValue)
        {
            // Brand
            if (includes.Value.HasFlag(VehicleIncludes.Brand))
            {
                query = query.Include(x => x.Brand);
            }
            // VehicleType
            if (includes.Value.HasFlag(VehicleIncludes.VehicleType))
            {
                query = query.Include(x => x.VehicleType);
            }
            // InterventionTypes
            if (includes.Value.HasFlag(VehicleIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes!)
                    .ThenInclude(x => x.InterventionType);
            }
            // Labels
            if (includes.Value.HasFlag(VehicleIncludes.Labels))
            {
                query = query.Include(x => x.Labels!.OrderBy(a => a.SortOrder));
            }
            // Attachments
            if (includes.Value.HasFlag(VehicleIncludes.Attachments))
            {
                query = query.Include(x => x.Attachments!)
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }
}