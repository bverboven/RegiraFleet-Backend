using Regira.Fleet.Abstractions;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Phone { get; set; }

    public ICollection<int>? InterventionTypeId { get; set; }
}