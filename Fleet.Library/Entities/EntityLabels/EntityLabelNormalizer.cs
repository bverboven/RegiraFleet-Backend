using Regira.Fleet.Models.EntityLabels;
using Regira.Globalization.LibPhoneNumber;
using Regira.Normalizing.Abstractions;
using Regira.Utilities;

namespace Regira.Fleet.Entities.EntityLabels;

public class EntityLabelNormalizer(INormalizer defaultNormalizer, PhoneNumberFormatter phoneNumberNormalizer) : IObjectNormalizer
{
    public bool IsExclusive => false;
    public INormalizer DefaultNormalizer => defaultNormalizer;

    public Task HandleNormalizeMany(IEnumerable<object?> instances, bool recursive = false)
    {
        foreach (var item in instances)
        {
            HandleNormalize(item, recursive);
        }
        return Task.CompletedTask;
    }
    public void HandleNormalize(object? instance, bool recursive = false)
    {
        if (instance is IEntityLabel label)
        {
            Normalize(label);
        }
    }

    public string? Normalize(IEntityLabel input)
    {
        var normalizedTitle = defaultNormalizer.Normalize(input.Title);
        var normalizedValue = NormalizeValue(input.Value);
        return $"{normalizedTitle} {normalizedValue}".Trim();
    }
    public string? NormalizeValue(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        if (RegexUtility.IsValidIPAddress(input))
        {
            return $"{defaultNormalizer.Normalize(input)} {input.Replace(".", "_")}";
        }
        if (RegexUtility.IsValidPhoneNumber(input))
        {
            try
            {
                return $"{defaultNormalizer.Normalize(input)} {phoneNumberNormalizer.Normalize(input)}";
            }
            catch
            {
                // ignore
            }
        }
        if (RegexUtility.IsValidEmail(input))
        {
            return $"{defaultNormalizer.Normalize(input)} {input.ToUpper()}";
        }
        if (RegexUtility.IsValidUrl(input))
        {
            return $"{defaultNormalizer.Normalize(input)} {input.ToUpper()}";
        }

        return defaultNormalizer.Normalize(input);
    }
    public void NormalizeItem(IHasLabels item)
    {
        if (item.Labels?.Any() != true)
        {
            return;
        }

        foreach (var data in item.Labels!)
        {
            data.NormalizedContent = Normalize(data);
        }
    }
}
