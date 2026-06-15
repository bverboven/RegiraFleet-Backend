using Regira.Fleet.Models.Abstractions;

namespace Regira.Fleet.Models.InterventionOperators.Operators;

public record OperatorSearchObject : FleetSearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public ICollection<int>? InterventionTypeId { get; set; }

    public bool? HasIntervention { get; set; }
}