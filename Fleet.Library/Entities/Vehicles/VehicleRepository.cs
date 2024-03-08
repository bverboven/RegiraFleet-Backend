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
                query = query.Where(x => EF.Functions.ILike(x.Brand!.Code!, so.Brand) ||
                    EF.Functions.ILike(x.Brand!.Title!, so.Brand));
            }
            // VehicleTypeId
            if (so.VehicleTypeId?.Any() == true)
            {
                query = query.Where(x => so.VehicleTypeId.Contains(x.VehicleTypeId!.Value));
            }
            // VehicleType
            if (!string.IsNullOrWhiteSpace(so.VehicleType))
            {
                query = query.Where(x => EF.Functions.ILike(x.VehicleType!.Code!, so.VehicleType) ||
                    EF.Functions.ILike(x.VehicleType!.Title!, so.VehicleType));
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var kw = qHelper.ParseKeyword(so.Title.ToUpper());
                query = query.Where(x => EF.Functions.ILike(x.NormalizedTitle!, kw.Q!));
            }
            // HasIntervention
            if (so.HasIntervention.HasValue)
            {
                query = query.Where(x => DbContext.Interventions.Any(i => i.VehicleId == x.Id));
            }
            // Q
            query = query.FilterQ(qHelper.Parse(so.Q?.ToUpper()));
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