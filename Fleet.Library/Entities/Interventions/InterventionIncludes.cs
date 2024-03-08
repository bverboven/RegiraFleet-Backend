namespace Regira.Fleet.Entities.Interventions;

[Flags]
public enum InterventionIncludes
{
    None = 0,
    Invoices = 1 << 0,
    Vehicle = 1 << 1,
    Operator = 1 << 2,
    InterventionTypes = 1 << 3,
    Attachments = 1 << 4,
    All = Invoices | Vehicle | Operator | InterventionTypes | Attachments
}
