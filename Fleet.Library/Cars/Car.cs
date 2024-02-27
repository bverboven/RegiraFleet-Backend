using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Brands;
using Regira.Fleet.CarTypes;

namespace Regira.Fleet.Cars;

public class Car : IEntityWithSerial, IHasCode, IArchivable
{
    public int Id { get; set; }
    public int? BrandId { get; set; }
    public int? CarTypeId { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string? Model { get; set; }
    public bool IsArchived { get; set; }

    public virtual Brand? Brand { get; set; }
    public virtual CarType? CarType { get; set; }
}