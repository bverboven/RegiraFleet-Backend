using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Entities.InterventionTypes;

namespace Regira.Fleet.Entities.Interventions;

[PrimaryKey(nameof(InterventionId), nameof(InterventionTypeId))]
public class InterventionInterventionType
{
    public int InterventionId { get; set; }
    public int InterventionTypeId { get; set; }

    public Intervention? Intervention { get; set; }
    public InterventionType? InterventionType { get; set; }
}
