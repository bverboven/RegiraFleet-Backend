using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Models.InterventionOperators.Operators;

public class OperatorInterventionTypeDto
{
    public int Id { get; set; }
    public int OperatorId { get; set; }
    public int InterventionTypeId { get; set; }

    public InterventionTypeDto? InterventionType { get; set; }
}