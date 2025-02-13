using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.EFcore.Normalizing.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Core.Normalizing;


public class FleetEntityNormalizer<T>(INormalizer normalizer)
    : EntityNormalizerBase<T>(normalizer)
    where T : class
{
    /// <summary>
    /// Gets entries for
    /// <list type="bullet">
    ///     <item><see cref="IHasCode"/></item>
    ///     <item><see cref="IHasTitle"/></item>
    ///     <item><see cref="IHasNormalizedTitle"/></item>
    ///     <item><see cref="IHasDescription"/></item>
    ///     <item><see cref="IHasAttachments"/></item>
    /// </list>
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public List<string?> GetDefaultNormalizedContentEntries(object item)
    {
        var entries = new List<string?>();

        if (item is IHasCode hasCode)
        {
            entries.Add(hasCode.Code?.ToUpper());
        }
        if (item is IHasNormalizedTitle hasNormalizedTitle)
        {
            entries.Add(hasNormalizedTitle.NormalizedTitle);
        }
        else if (item is IHasTitle hasTitle)
        {
            entries.Add(DefaultPropertyNormalizer.Normalize(hasTitle.Title));
        }
        if (item is IHasDescription hasDescription)
        {
            entries.Add(DefaultPropertyNormalizer.Normalize(hasDescription.Description));
        }

        if (item is IHasAttachments hasAttachments)
        {
            if (hasAttachments.Attachments?.Any() == true)
            {
                entries.AddRange(hasAttachments.Attachments.Select(a =>
                    DefaultPropertyNormalizer.Normalize(Path.GetFileNameWithoutExtension(a.Attachment?.FileName))
                ));
            }
        }

        return entries;
    }
}