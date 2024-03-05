using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Entities.InterventionTypes;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

[PrimaryKey(nameof(OperatorId), nameof(InterventionTypeId))]
public class OperatorInterventionType
{
    public int OperatorId { get; set; }
    public int InterventionTypeId { get; set; }

    public Operator? Operator { get; set; }
    public InterventionType? InterventionType { get; set; }
}
