namespace Regira.Fleet.Models.Vehicles;

public class VehicleInterventionTypeInputDto
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public int InterventionTypeId { get; set; }
}