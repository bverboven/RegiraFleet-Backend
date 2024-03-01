namespace Regira.Fleet.Entities.Interventions.Actions;

[Flags]
public enum InterventionActionIncludes
{
    None = 0,
    Invoices = 1 << 0,
    Vehicles = 1 << 1,
    Operators = 1 << 2,
    InterventionTypes = 1 << 3,
    Attachments = 1 << 4,
    All = Vehicles | Operators | InterventionTypes | Attachments
}