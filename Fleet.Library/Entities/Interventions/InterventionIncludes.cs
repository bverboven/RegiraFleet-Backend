namespace Regira.Fleet.Entities.Interventions;

[Flags]
public enum InterventionIncludes
{
    None = 0,
    Vehicles = 1 << 0,
    Suppliers = 1 << 1,
    InterventionTypes = 1 << 2,
    Attachments = 1 << 3,
    All = Vehicles | Suppliers | InterventionTypes | Attachments
}