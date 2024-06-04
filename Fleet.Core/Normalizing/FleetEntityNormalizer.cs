using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Core.Normalizing.Abstractions;
using Regira.Normalizing;
using Regira.Normalizing.Abstractions;
using Regira.Normalizing.Models;

namespace Regira.Fleet.Core.Normalizing;

//public class FleetEntityNormalizer(INormalizer normalizer)
//    : ObjectNormalizer(new NormalizingOptions { DefaultNormalizer = normalizer }), IFleetEntityNormalizer
//{
//    void IObjectNormalizer.HandleNormalize(object? item, bool recursive) => HandleNormalize(item, recursive);
//    public override void HandleNormalize(object? item, bool recursive = true)
//    {
//        if (item == null)
//        {
//            return;
//        }

//        base.HandleNormalize(item, recursive);

//        SetNormalizedContent(item);
//    }

//}

public class FleetEntityNormalizer<T>(INormalizer normalizer)
    : ObjectNormalizer<T>
    where T : class
{
    public new INormalizer DefaultNormalizer => normalizer;

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
            entries.Add(DefaultNormalizer.Normalize(hasTitle.Title));
        }
        if (item is IHasDescription hasDescription)
        {
            entries.Add(DefaultNormalizer.Normalize(hasDescription.Description));
        }

        //var normalizedProps = item.GetType().GetProperties().WithCustomAttribute<NormalizedAttribute>();
        //foreach (var normalizedProp in normalizedProps)
        //{
        //    var value = normalizedProp.GetValue(item)?.ToString();
        //    if (!string.IsNullOrWhiteSpace(value) && !entries.Contains(value))
        //    {
        //        entries.Add(value);
        //    }
        //}

        if (item is IHasAttachments hasAttachments)
        {
            if (hasAttachments.Attachments?.Any() == true)
            {
                entries.AddRange(hasAttachments.Attachments.Select(a =>
                    DefaultNormalizer.Normalize(Path.GetFileNameWithoutExtension(a.Attachment?.FileName))
                ));
            }
        }

        return entries;
    }
}