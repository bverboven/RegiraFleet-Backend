namespace Regira.Fleet.Entities.Interventions;

[Flags]
public enum InterventionIncludes
{
    None = 0,
    Invoice = 1 << 0,
    Vehicle = 1 << 1,
    Operator = 1 << 2,
    InterventionType = 1 << 3,
    Attachments = 1 << 4,
    All = Invoice | Vehicle | Operator | InterventionType | Attachments
}
