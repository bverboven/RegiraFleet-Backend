using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Models.Translations;

public interface IHasTranslations
{
    public ICollection<Translation>? Translations { get; set; }
}
public interface IHasTranslations<T> : IHasTitle
    where T : Translation
{
    public ICollection<T>? Translations { get; set; }
}
