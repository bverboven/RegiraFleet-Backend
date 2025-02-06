using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleRepository(FleetContextBase dbContext, IFleetAppContext appContext,
    IQueryBuilder<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes> queryBuilder)
    : FleetRepositoryBase<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes>(dbContext, queryBuilder, appContext)
{
    private readonly FleetContextBase _dbContext1 = dbContext;

    public override void Modify(Vehicle item, Vehicle original)
    {
        base.Modify(item, original);

        if (item.InterventionTypes != null)
        {
            var itemsToRemove = original.InterventionTypes?
                .Where(o => item.InterventionTypes.All(x => o.InterventionTypeId != x.InterventionTypeId))
                .ToArray() ?? [];
            var itemsToAdd = item.InterventionTypes
                .Where(x => original.InterventionTypes == null || original.InterventionTypes.All(o => x.InterventionTypeId != o.InterventionTypeId))
                .ToArray();
            foreach (var itemToRemove in itemsToRemove)
            {
                _dbContext1.Entry(itemToRemove).State = EntityState.Deleted;
            }
            foreach (var itemToAdd in itemsToAdd)
            {
                _dbContext1.Entry(itemToAdd).State = EntityState.Added;
            }
            original.InterventionTypes = (original.InterventionTypes ?? Array.Empty<VehicleInterventionType>())
                .Except(itemsToRemove)
                .Concat(itemsToAdd)
                .ToList();
        }

        _dbContext1.UpdateEntityChildCollection(original, item, x => x.Labels, (x, collection) => x.Labels = collection);

        if (item.Attachments != null)
        {
            _dbContext1.ModifyEntityAttachments(original, item);
        }
    }
    public override void PrepareItem(Vehicle item)
    {
        base.PrepareItem(item);

        item.Labels?.Prepare();
    }
}