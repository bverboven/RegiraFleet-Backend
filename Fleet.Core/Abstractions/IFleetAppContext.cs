namespace Regira.Fleet.Core.Abstractions;

public interface IFleetAppContext
{
    IClientContext Client { get; }
    ICultureContext Culture { get; }
}
