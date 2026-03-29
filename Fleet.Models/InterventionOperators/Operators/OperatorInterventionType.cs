using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Models.InterventionOperators.Operators;

public class OperatorInterventionType : IEntityWithSerial
{
    public int Id { get; set; }
    public int OperatorId { get; set; }
    public int InterventionTypeId { get; set; }

    public InterventionType? InterventionType { get; set; }
}