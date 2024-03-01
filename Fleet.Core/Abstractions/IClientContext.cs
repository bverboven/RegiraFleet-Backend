namespace Regira.Fleet.Core.Abstractions;

public interface IClientContext
{
    int ClientId { get; }

    Task Load(string clientId);
}