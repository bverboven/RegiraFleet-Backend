using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Models.Abstractions;

public interface IFleetEntity : IEntity<int>, IHasTimestamps, IHasTenantId
{
    string Guid { get; set; }
}