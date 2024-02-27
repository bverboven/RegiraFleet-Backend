using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Models;
using Regira.Fleet.DAL.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Cars;

public class CarRepository(FleetContext dbContext) : DefaultFleetRepository<Car, CarSearchObject>(dbContext)
{
    public override IQueryable<Car> Filter(IQueryable<Car> query, CarSearchObject? so)
    {
        query = base.Filter(query, so);

        if(so!=null)
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
            
            if (!string.IsNullOrWhiteSpace(so.CarType))
            {
                query = query.Where(x =>
                    x.CarType!.Code!.Equals(so.CarType, StringComparison.InvariantCultureIgnoreCase) ||
                    x.CarType.Title!.Equals(so.CarType, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        return query;
    }

    public override IQueryable<Car> SortBy(IQueryable<Car> query, EntitySortBy? sortBy = null)
    {
        return query.OrderBy(x => x.Code);
    }
}