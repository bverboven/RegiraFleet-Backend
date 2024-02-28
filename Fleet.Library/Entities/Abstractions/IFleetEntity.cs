using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Abstractions;

public interface IFleetEntity : IEntity<int>, IHasTimestamps
{
    [MinLength(32), MaxLength(32)]
    public string Guid { get; set; }
    public int ClientId { get; set; }
}