namespace Regira.Fleet.Entities.Vehicles;

[Flags]
public enum VehicleIncludes
{
    None = 0,
    Brand = 1 << 0,
    VehicleType = 1 << 1,
    InterventionTypes = 1 << 2,
    Attachments = 1 << 3,
    //Interventions = 1 << 4,
    All = Brand | VehicleType | InterventionTypes | Attachments //| Interventions
}
