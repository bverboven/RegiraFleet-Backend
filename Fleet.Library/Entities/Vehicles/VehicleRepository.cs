using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<Vehicle, VehicleSearchObject>(dbContext, appContext)
{
    public override IQueryable<Vehicle> Filter(IQueryable<Vehicle> query, VehicleSearchObject? so)
    {
        query = base.Filter(query, so);

        if (so != null)
        {
            query = query.FilterArchivable(so.IsArchived);
            query = query.FilterCode(so.Code);

            if (!string.IsNullOrWhiteSpace(so.Model))
            {
                query = query.Where(x => x.Model!.Equals(so.Model, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(so.Brand))
            {
                query = query.Where(x =>
                    x.Brand!.Title!.Equals(so.Brand, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Brand.Code!.Equals(so.Brand, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(so.VehicleType))
            {
                query = query.Where(x =>
                    x.VehicleType!.Code!.Equals(so.VehicleType, StringComparison.InvariantCultureIgnoreCase) ||
                    x.VehicleType.Title!.Equals(so.VehicleType, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        return query;
    }

    public override IQueryable<Vehicle> SortBy(IQueryable<Vehicle> query, EntitySortBy? sortBy = null)
    {
        return query.OrderBy(x => x.Code);
    }
}