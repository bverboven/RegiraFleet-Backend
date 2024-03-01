using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Normalizing.Abstractions;

public abstract class FleetEntityNormalizerBase<T>(INormalizer normalizer)
    : FleetEntityNormalizer(normalizer), IFleetEntityNormalizer<T>
    where T : class
{
    public override void HandleNormalize(object? item, bool recursive = true)
        => HandleNormalize(item as T);
    public virtual void HandleNormalize(T? item)
    {
        base.HandleNormalize(item);
    }

    void IFleetEntityNormalizer.SetNormalizedContent(object item)
        => SetNormalizedContent((T)item);

    public override void SetNormalizedContent(object item)
        => SetNormalizedContent((T)item);
    public virtual void SetNormalizedContent(T item)
        => base.SetNormalizedContent(item);
}