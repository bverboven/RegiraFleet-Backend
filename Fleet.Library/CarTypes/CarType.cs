using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.CarTypes;

public class CarType : IEntityWithSerial, IHasCode, IHasTitle, IArchivable
{
    public int Id { get; set; }
    [MaxLength(3)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string? Title { get; set; }
    public bool IsArchived { get; set; }
}