using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
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
            if (includes.Value.HasFlag(InterventionIncludes.Invoice))
            {
                query = query
                    .Include(x => x.Invoice);
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
            if (includes.Value.HasFlag(InterventionIncludes.InterventionType))
            {
                query = query
                    .Include(x => x.InterventionType);
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
        if (item.Invoice != null)
        {
            original.Invoice = item.Invoice;
            DbContext.Entry(item.Invoice).State = original.Invoice.Id > 0 ? EntityState.Modified : EntityState.Added;
        }
        if (original.Invoice != null && item.Invoice == null)
        {
            DbContext.Entry(original.Invoice).State = EntityState.Deleted;
        }

        if (item.Attachments != null)
        {
            DbContext.ModifyEntityAttachments(original, item);
        }

        base.Modify(item, original);
    }
    public override void PrepareItem(Intervention item)
    {
        base.PrepareItem(item);

        if (item.Invoice != null)
        {
            item.Invoice.InterventionId = item.Id;
        }
    }
}