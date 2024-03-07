using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Normalizing;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.InterventionOperators.Normalizers;

public class OperatorNormalizer(INormalizer defaultNormalizer, IdentificationNumberNormalizer idNumberNormalizer,
    ContactDataNormalizer contactDataNormalizer, AddressNormalizer addressNormalizer, ICultureContext cultureContext)
    : FleetEntityNormalizer<Operator>(defaultNormalizer)
{
    public override void HandleNormalize(Operator? item)
    {
        if (item == null)
        {
            return;
        }

        // KBO
        item.NormalizedIdentificationNumber = idNumberNormalizer.Normalize(item.IdentificationNumber);

        // ContactData
        if (item.ContactData?.Any() == true)
        {
            foreach (var contactData in item.ContactData)
            {
                contactDataNormalizer.Normalize(contactData);
            }
        }

        // Address
        if (item.Addresses?.Any() == true)
        {
            foreach (var address in item.Addresses)
            {
                addressNormalizer.Normalize(address, cultureContext.LangCode);
            }
        }


        // Title
        item.NormalizedTitle = DefaultNormalizer.Normalize(item.Title);

        // NormalizedContent
        SetNormalizedContent(item);
    }

    public override void SetNormalizedContent(Operator item)
    {
        var contentEntries = GetDefaultNormalizedContentEntries(item);
        contentEntries.AddRange(new[]
        {
            item.NormalizedIdentificationNumber?.ToUpper()
        });

        if (item.ContactData?.Any() == true)
        {
            contentEntries.AddRange(item.ContactData.Select(a => a.NormalizedValue));
        }
        if (item.Addresses?.Any() == true)
        {
            contentEntries.AddRange(item.Addresses.Select(a => a.NormalizedContent));
        }

        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}