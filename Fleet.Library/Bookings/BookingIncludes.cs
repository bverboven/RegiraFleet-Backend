namespace Regira.Fleet.Bookings;

[Flags]
public enum BookingIncludes
{
    None = 0,
    Cars = 1 << 0,
    Suppliers = 1 << 1,
    InterventionTypes = 1 << 2,
    All = Cars | Suppliers | InterventionTypes
}