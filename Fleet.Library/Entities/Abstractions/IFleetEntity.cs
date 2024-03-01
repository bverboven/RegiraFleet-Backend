using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Entities.Abstractions;

public interface IFleetEntity : IEntity<int>, IHasTimestamps
{
    public string Guid { get; set; }
    public int ClientId { get; set; }
}