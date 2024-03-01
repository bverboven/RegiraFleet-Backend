using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Core.Models;

public class FleetAppContext(IClientContext clientContext, ICultureContext cultureContext) : IFleetAppContext
{
    public IClientContext Client => clientContext;
    public ICultureContext Culture => cultureContext;
}
