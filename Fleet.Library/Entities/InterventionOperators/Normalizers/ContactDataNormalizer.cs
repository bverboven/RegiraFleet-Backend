using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Globalization.LibPhoneNumber;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.InterventionOperators.Normalizers;

public class ContactDataNormalizer(INormalizer defaultNormalizer, PhoneNumberFormatter phoneNumberNormalizer)
    : FleetEntityNormalizer<OperatorContactData>(defaultNormalizer)
{
    public override Task HandleNormalizeMany(IEnumerable<OperatorContactData> items, CancellationToken cancellationToken = default)
    {
        foreach (var item in items)
        {
            HandleNormalize(item, cancellationToken);
        }
        return Task.CompletedTask;
    }
    public override Task HandleNormalize(OperatorContactData item, CancellationToken cancellationToken = default)
    {
        item.NormalizedValue = Normalize(item);
        return Task.CompletedTask;
    }

    public string? Normalize(string? input, ContactDataTypes dataType)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        switch (dataType)
        {
            case ContactDataTypes.Phone:
                try
                {
                    return phoneNumberNormalizer.Normalize(input);
                }
                catch
                {
                    return null;
                }
            case ContactDataTypes.Email:
                return input.ToUpper();
            case ContactDataTypes.Website:
                return input.ToUpper();
            default: //case ContactDataTypes.Other
                return DefaultPropertyNormalizer.Normalize(input);
        }
    }
    public string? Normalize(OperatorContactData data)
    {
        data.NormalizedValue = Normalize(data.Value, data.DataType);
        return data.NormalizedValue;
    }
}