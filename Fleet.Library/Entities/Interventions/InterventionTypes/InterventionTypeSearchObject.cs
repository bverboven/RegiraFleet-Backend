using Regira.Entities.Models;

namespace Regira.Fleet.Entities.Interventions.InterventionTypes;

public class InterventionTypeSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
}