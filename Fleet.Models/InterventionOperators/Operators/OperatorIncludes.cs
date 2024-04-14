namespace Regira.Fleet.Models.InterventionOperators.Operators;

[Flags]
public enum OperatorIncludes
{
    None = 0,
    Addresses = 1 << 0,
    ContactData = 1 << 1,
    InterventionTypes = 1 << 2,
    Labels = 1 << 3,
    Attachments = 1 << 4,
    All = Addresses | ContactData | InterventionTypes | Labels | Attachments
}