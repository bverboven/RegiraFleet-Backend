namespace Regira.Fleet.Entities.InterventionOperators.Operators;

[Flags]
public enum InterventionOperatorIncludes
{
    None = 0,
    Addresses = 1 << 0,
    ContactData = 1 << 1,
    InterventionTypes = 1 << 2,
    Attachments = 1 << 3,
    All = Addresses | ContactData | InterventionTypes | Attachments
}