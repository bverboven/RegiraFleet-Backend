using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Models.EntityLabels;
using Regira.Globalization.LibPhoneNumber;
using Regira.Normalizing.Abstractions;
using Regira.Utilities;

namespace Regira.Fleet.Entities.EntityLabels;

public class EntityLabelNormalizer(INormalizer defaultNormalizer, PhoneNumberFormatter phoneNumberNormalizer)
    : FleetEntityNormalizer<IEntityLabel>(defaultNormalizer)
{
    public override Task HandleNormalizeMany(IEnumerable<IEntityLabel> instances)
    {
        foreach (var item in instances)
        {
            HandleNormalize(item);
        }
        return Task.CompletedTask;
    }
    public override Task HandleNormalize(IEntityLabel item)
    {
        var normalizedTitle = DefaultPropertyNormalizer.Normalize(item.Title);
        var normalizedValue = NormalizeValue(item.Value);
        item.NormalizedContent = $"{normalizedTitle} {normalizedValue}".Trim();

        return Task.CompletedTask;
    }

    public string? NormalizeValue(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        if (RegexUtility.IsValidIPAddress(input))
        {
            return $"{DefaultPropertyNormalizer.Normalize(input)} {input.Replace(".", "_")}";
        }
        if (RegexUtility.IsValidPhoneNumber(input))
        {
            try
            {
                return $"{DefaultPropertyNormalizer.Normalize(input)} {phoneNumberNormalizer.Normalize(input)}";
            }
            catch
            {
                // ignore
            }
        }
        if (RegexUtility.IsValidEmail(input))
        {
            return $"{DefaultPropertyNormalizer.Normalize(input)} {input.ToUpper()}";
        }
        if (RegexUtility.IsValidUrl(input))
        {
            return $"{DefaultPropertyNormalizer.Normalize(input)} {input.ToUpper()}";
        }

        return DefaultPropertyNormalizer.Normalize(input);
    }
}
