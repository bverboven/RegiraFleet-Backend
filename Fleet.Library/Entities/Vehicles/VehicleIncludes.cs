namespace Regira.Fleet.Entities.Vehicles;

[Flags]
public enum VehicleIncludes
{
    None = 0,
    Brand = 1 << 0,
    VehicleType = 1 << 1,
    All = Brand | VehicleType
}
