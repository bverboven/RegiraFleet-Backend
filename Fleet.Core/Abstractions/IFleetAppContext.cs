namespace Regira.Fleet.Core.Abstractions;

public interface IFleetAppContext
{
    ITenantContext Tenant { get; }
    ICultureContext Culture { get; }
}
