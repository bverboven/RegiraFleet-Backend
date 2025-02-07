using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Attachments;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Extensions;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionRepository(FleetContextBase dbContext, IFleetAppContext appContext,
    IQueryBuilder<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes> queryBuilder)
    : FleetRepositoryBase<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(dbContext, queryBuilder, appContext)
{
    private readonly FleetContextBase _dbContext1 = dbContext;

    public override void Modify(Intervention item, Intervention original)
    {
        if (item.Invoice != null)
        {
            original.Invoice = item.Invoice;
            _dbContext1.Entry(item.Invoice).State = original.Invoice.Id > 0 ? EntityState.Modified : EntityState.Added;
        }
        if (original.Invoice != null && item.Invoice == null)
        {
            _dbContext1.Entry(original.Invoice).State = EntityState.Deleted;
        }

        _dbContext1.UpdateEntityChildCollection(original, item, x => x.Labels, (x, collection) => x.Labels = collection);

        if (item.Attachments != null)
        {
            _dbContext1.ModifyEntityAttachments(original, item);
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

        item.Labels?.Prepare();
    }
}