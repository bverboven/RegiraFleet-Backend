using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleTypeInputDto
{
    [Required]
    [MaxLength(3)]
    public string Code { get; set; } = null!;
    [Required]
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    public bool IsArchived { get; set; }
}