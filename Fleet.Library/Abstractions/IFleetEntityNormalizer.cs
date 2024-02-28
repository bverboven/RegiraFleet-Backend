using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Abstractions;

public interface IFleetEntityNormalizer : IObjectNormalizer
{
    void SetNormalizedContent(object item);
}
public interface IFleetEntityNormalizer<in T> : IFleetEntityNormalizer
    where T : class
{
    void HandleNormalize(T? item);
    void SetNormalizedContent(T item);
    List<string?> GetDefaultNormalizedContentEntries(object item);
}