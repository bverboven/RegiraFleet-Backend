using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Models.InterventionOperators.Operators;

[PrimaryKey(nameof(OperatorId), nameof(InterventionTypeId))]
public class OperatorInterventionType
{
    public int OperatorId { get; set; }
    public int InterventionTypeId { get; set; }

    public Operator? Operator { get; set; }
    public InterventionType? InterventionType { get; set; }
}
