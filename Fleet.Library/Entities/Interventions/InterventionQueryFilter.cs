using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionQueryFilter(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Intervention, InterventionSearchObject>
{
    public override IQueryable<Intervention> Build(IQueryable<Intervention> query, InterventionSearchObject? so)
    {
        if (so != null)
        {
            // OperatorId
            if (so.OperatorId?.Any() == true)
            {
                query = query.Where(x => so.OperatorId.Contains(x.OperatorId));
            }
            // InterventionTypeId
            if (so.InterventionTypeId?.Any() == true)
            {
                query = query.Where(x => so.InterventionTypeId.Contains(x.InterventionTypeId!.Value));
            }
            // VehicleId
            if (so.VehicleId?.Any() == true)
            {
                query = query.Where(x => so.VehicleId.Contains(x.VehicleId));
            }
            // VehicleTypeId
            if (so.VehicleTypeId?.Any() == true)
            {
                query = query.Where(x => so.VehicleTypeId.Contains(x.Vehicle!.VehicleTypeId!.Value));
            }
            // BrandId
            if (so.BrandId?.Any() == true)
            {
                query = query.Where(x => so.BrandId.Contains(x.Vehicle!.BrandId!.Value));
            }
            // MinDate
            if (so.MinDate.HasValue)
            {
                query = query.Where(x => so.MinDate <= x.InterventionDate);
            }
            // MaxDate
            if (so.MaxDate.HasValue)
            {
                query = query.Where(x => so.MaxDate >= x.InterventionDate);
            }
            // Q
            query = query.FilterILikeQ(qHelper.Parse(so.Q));
        }

        return query;
    }
}