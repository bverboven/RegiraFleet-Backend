using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleQueryBuilder(IEnumerable<IGlobalFilteredQueryBuilder> globalFilters,
    IEnumerable<IFilteredQueryBuilder<Vehicle, VehicleSearchObject>>? filters = null)
    : QueryBuilder<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes>(globalFilters, filters)
{
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