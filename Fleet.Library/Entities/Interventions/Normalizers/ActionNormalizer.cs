using Regira.Fleet.Normalizing.Abstractions;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Interventions.Normalizers;

public class ActionNormalizer(INormalizer normalizer) : FleetEntityNormalizerBase<Intervention>(normalizer)
{
    public override void HandleNormalize(Intervention? item)
    {
        base.HandleNormalize(item);
    }
    public override void SetNormalizedContent(Intervention item)
    {
        base.SetNormalizedContent(item);
    }
}