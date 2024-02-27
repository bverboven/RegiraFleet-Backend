using Regira.Entities.Models;

namespace Regira.Fleet.InterventionTypes;

public class InterventionTypeSearchObject : SearchObject
{
    public string? Code { get; set; }
    public string? Title { get; set; }
}