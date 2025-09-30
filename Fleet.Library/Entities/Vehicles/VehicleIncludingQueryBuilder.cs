using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleIncludingQueryBuilder()
    : IIncludableQueryBuilder<Vehicle, int, VehicleIncludes>
{
    public IQueryable<Vehicle> AddIncludes(IQueryable<Vehicle> query, VehicleIncludes? includes = null)
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
                query = query.Include(x => x.Attachments!.OrderBy(a => a.SortOrder))
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }
}