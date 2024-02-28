using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Entities.Abstractions;
using Regira.Fleet.Entities.Cars.Brands;
using Regira.Fleet.Entities.Cars.CarTypes;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Cars;

public class Car : IFleetEntity, IEntityWithSerial, IHasCode, IArchivable
{
    public int Id { get; set; }
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    public int ClientId { get; set; }
    public int? BrandId { get; set; }
    public int? CarTypeId { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string? Model { get; set; }
    public string? Notes { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public virtual Brand? Brand { get; set; }
    public virtual CarType? CarType { get; set; }

    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }

}