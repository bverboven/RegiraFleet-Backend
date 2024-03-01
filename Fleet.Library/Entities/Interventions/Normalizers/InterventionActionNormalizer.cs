using Regira.Fleet.Entities.Interventions.Actions;
using Regira.Fleet.Normalizing.Abstractions;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Interventions.Normalizers;

public class InterventionActionNormalizer(INormalizer normalizer) : FleetEntityNormalizerBase<InterventionAction>(normalizer)
{
    public override void HandleNormalize(InterventionAction? item)
    {
        base.HandleNormalize(item);
    }
    public override void SetNormalizedContent(InterventionAction item)
    {
        base.SetNormalizedContent(item);
    }
}