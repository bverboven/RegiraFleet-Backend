namespace Regira.Fleet.Models.Vehicles.VehicleTypes;

public class VehicleTypeDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }
}