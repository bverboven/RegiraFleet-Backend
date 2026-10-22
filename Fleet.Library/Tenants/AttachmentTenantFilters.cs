using Regira.Entities.Models.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Tenants;

// An attachment link has no tenant of its own, so the tenant filter never sees it: these scope each link type to the links
// whose owner belongs to the active tenant. They cover every read of a link (the owner's attachments, a download, the list)
// and the lookup a link's update or delete starts from.
// One class per link type, and no owner type among their generic arguments: a global filter applies to every entity
// its type names, so naming the owner would run it on the owner's own queries.

public class VehicleAttachmentTenantFilter(FleetContextBase dbContext, IFleetAppContext appContext) : GlobalFilteredQueryBuilderBase<VehicleAttachment>
{
    public override IQueryable<VehicleAttachment> Build(IQueryable<VehicleAttachment> query, ISearchObject<int>? _)
        => query.Where(a => dbContext.Vehicles.Any(o => o.Id == a.ObjectId && o.TenantId == appContext.Tenant.TenantId));
}

public class OperatorAttachmentTenantFilter(FleetContextBase dbContext, IFleetAppContext appContext) : GlobalFilteredQueryBuilderBase<OperatorAttachment>
{
    public override IQueryable<OperatorAttachment> Build(IQueryable<OperatorAttachment> query, ISearchObject<int>? _)
        => query.Where(a => dbContext.InterventionOperators.Any(o => o.Id == a.ObjectId && o.TenantId == appContext.Tenant.TenantId));
}

public class InterventionAttachmentTenantFilter(FleetContextBase dbContext, IFleetAppContext appContext) : GlobalFilteredQueryBuilderBase<InterventionAttachment>
{
    public override IQueryable<InterventionAttachment> Build(IQueryable<InterventionAttachment> query, ISearchObject<int>? _)
        => query.Where(a => dbContext.Interventions.Any(o => o.Id == a.ObjectId && o.TenantId == appContext.Tenant.TenantId));
}
