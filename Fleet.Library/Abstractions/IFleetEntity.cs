using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Abstractions;

public interface IFleetEntity : IEntity<int>, IHasTimestamps, IHasClientId
{
    string Guid { get; set; }
}
