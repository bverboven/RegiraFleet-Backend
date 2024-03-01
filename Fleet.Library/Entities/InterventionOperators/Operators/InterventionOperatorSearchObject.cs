using Regira.Entities.Models;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class InterventionOperatorSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Phone { get; set; }
}