namespace Regira.Fleet.Models.Vehicles;

[Flags]
public enum VehicleIncludes
{
    None = 0,
    Brand = 1 << 0,
    VehicleType = 1 << 1,
    InterventionTypes = 1 << 2,
    Labels = 1 << 3,
    Attachments = 1 << 4,
    All = Brand | VehicleType | InterventionTypes | Labels | Attachments
}
