using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Core.Models;

public class FleetAppContext(ITenantContext tenantContext, ICultureContext cultureContext) : IFleetAppContext
{
    public ITenantContext Tenant => tenantContext;
    public ICultureContext Culture => cultureContext;
}
