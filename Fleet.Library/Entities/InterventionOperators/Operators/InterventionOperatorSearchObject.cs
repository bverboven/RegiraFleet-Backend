using Regira.Fleet.Abstractions;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class InterventionOperatorSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Phone { get; set; }
}