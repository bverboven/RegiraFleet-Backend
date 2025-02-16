using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleFilteredQueryBuilder(FleetContextBase dbContext) : FilteredQueryBuilderBase<Vehicle, VehicleSearchObject>
{
    public override IQueryable<Vehicle> Build(IQueryable<Vehicle> query, VehicleSearchObject? so)
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
            // VehicleTypeId
            if (so.VehicleTypeId?.Any() == true)
            {
                query = query.Where(x => so.VehicleTypeId.Contains(x.VehicleTypeId!.Value));
            }
            // HasIntervention
            if (so.HasIntervention.HasValue)
            {
                query = query.Where(x => dbContext.Interventions.Any(i => i.VehicleId == x.Id));
            }
        }

        return query;
    }
}