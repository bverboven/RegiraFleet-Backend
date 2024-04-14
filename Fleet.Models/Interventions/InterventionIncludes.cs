namespace Regira.Fleet.Models.Interventions;

[Flags]
public enum InterventionIncludes
{
    None = 0,
    Invoice = 1 << 0,
    Vehicle = 1 << 1,
    Operator = 1 << 2,
    InterventionType = 1 << 3,
    Labels = 1 << 4,
    Attachments = 1 << 5,
    All = Invoice | Vehicle | Operator | InterventionType | Labels | Attachments
}
