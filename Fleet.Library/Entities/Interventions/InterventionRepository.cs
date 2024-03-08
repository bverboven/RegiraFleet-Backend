using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.Keywords;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(dbContext, appContext)
{
    public override IQueryable<Intervention> Filter(IQueryable<Intervention> query, InterventionSearchObject? so)
    {
        query = base.Filter(query, so);

        if (so != null)
        {
            var qHelper = QKeywordHelper.Create();

            // OperatorId
            if (so.OperatorId?.Any() == true)
            {
                query = query.Where(x => so.OperatorId.Contains(x.OperatorId));
            }
            // InterventionTypeId
            if (so.InterventionTypeId?.Any() == true)
            {
                query = query.Where(x => x.InterventionTypes!.Any(it => so.InterventionTypeId.Contains(it.InterventionTypeId)));
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
    public override IQueryable<Intervention> SortBy(IQueryable<Intervention> query, InterventionSortBy? sortBy = null)
    {
        return query
            .OrderByDescending(x => x.InterventionDate ?? x.Created)
            //.OrderByDescending(x => x.Invoices!.Max(i => i.InvoiceDate))
            .ThenByDescending(x => x.Id);
    }
    public override IQueryable<Intervention> AddIncludes(IQueryable<Intervention> query, InterventionIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(InterventionIncludes.Invoices))
            {
                query = query
                    .Include(x => x.Invoices);
            }
            if (includes.Value.HasFlag(InterventionIncludes.Vehicle))
            {
                query = query
                    .Include(x => x.Vehicle!)
                    .ThenInclude(c => c.VehicleType)
                    .Include(x => x.Vehicle!)
                    .ThenInclude(c => c.Brand);
            }
            if (includes.Value.HasFlag(InterventionIncludes.Operator))
            {
                query = query
                    .Include(x => x.Operator!)
                    .ThenInclude(s => s.ContactData)
                    .Include(x => x.Operator!)
                    .ThenInclude(s => s.Addresses);
            }
            if (includes.Value.HasFlag(InterventionIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionTypes!)
                    .ThenInclude(x => x.InterventionType);
            }
            // Attachments
            if (includes.Value.HasFlag(InterventionIncludes.Attachments))
            {
                query = query.Include(x => x.Attachments!)
                    .ThenInclude(a => a.Attachment);
            }
        }

        return query;
    }

    public override void Modify(Intervention item, Intervention original)
    {
        if (item.InterventionTypes != null)
        {
            var itemsToRemove = original.InterventionTypes?
                .Where(o => item.InterventionTypes.All(x => o.InterventionTypeId != x.InterventionTypeId))
                .ToArray() ?? Array.Empty<InterventionInterventionType>();
            var itemsToAdd = item.InterventionTypes
                .Where(x => original.InterventionTypes == null || original.InterventionTypes.All(o => x.InterventionTypeId != o.InterventionTypeId))
                .ToArray();
            foreach (var itemToRemove in itemsToRemove)
            {
                DbContext.Entry(itemToRemove).State = EntityState.Deleted;
            }
            foreach (var itemToAdd in itemsToAdd)
            {
                DbContext.Entry(itemToAdd).State = EntityState.Added;
            }
            original.InterventionTypes = (original.InterventionTypes ?? Array.Empty<InterventionInterventionType>())
                .Except(itemsToRemove)
                .Concat(itemsToAdd)
                .ToList();
        }

        DbContext.UpdateEntityChildCollection(original, item, x => x.Invoices, (x, collection) => x.Invoices = collection);

        if (item.Attachments != null)
        {
            DbContext.ModifyEntityAttachments(original, item);
        }

        base.Modify(item, original);
    }
}