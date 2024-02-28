using Regira.Fleet.Entities.Suppliers.ContactData;
using Regira.Globalization.LibPhoneNumber;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Suppliers.Normalizers;

public class ContactDataNormalizer(INormalizer defaultNormalizer, PhoneNumberFormatter phoneNumberNormalizer) : IObjectNormalizer
{
    public INormalizer DefaultNormalizer => defaultNormalizer;

    public void HandleNormalize(object? instance, bool recursive = true)
    {
        if (instance is SupplierContactData data)
        {
            data.NormalizedValue = Normalize(data);
        }
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
                return defaultNormalizer.Normalize(input);
        }
    }
    public string? Normalize(SupplierContactData data)
        => Normalize(data.Value, data.DataType);
}