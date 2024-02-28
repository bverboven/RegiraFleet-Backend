namespace Regira.Fleet.Entities.Interventions;

[Flags]
public enum InterventionIncludes
{
    None = 0,
    Cars = 1 << 0,
    Suppliers = 1 << 1,
    InterventionTypes = 1 << 2,
    All = Cars | Suppliers | InterventionTypes
}