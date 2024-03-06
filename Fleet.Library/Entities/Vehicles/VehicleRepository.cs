using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Entities.Models;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Vehicles;

public class VehicleRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes>(dbContext, appContext)
{
    public override IQueryable<Vehicle> Filter(IQueryable<Vehicle> query, VehicleSearchObject? so)
    {
        query = base.Filter(query, so);

        if (so != null)
        {
            var qHelper = QKeywordHelper.Create();

            query = query.FilterArchivable(so.IsArchived);
            query = query.FilterCode(so.Code);
            query = query.FilterQ(qHelper.Parse(so.Q?.ToUpper()));

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

            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var kw = qHelper.ParseKeyword(so.Title.ToUpper());
                query = query.Where(x => EF.Functions.Like(x.NormalizedTitle, kw.QW));
            }
        }

        return query;
    }
    public override IQueryable<Vehicle> SortBy(IQueryable<Vehicle> query, EntitySortBy? sortBy = null)
    {
        return query.OrderBy(x => x.Code);
    }
    public override IQueryable<Vehicle> AddIncludes(IQueryable<Vehicle> query, VehicleIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

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
            // Attachments
            if (includes.Value.HasFlag(VehicleIncludes.Attachments))
            {
                query = query.Include(x => x.Attachments!)
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }

    public override void Modify(Vehicle item, Vehicle original)
    {
        base.Modify(item, original);

        if (item.Attachments != null)
        {
            DbContext.ModifyEntityAttachments(original, item);
        }
    }
}