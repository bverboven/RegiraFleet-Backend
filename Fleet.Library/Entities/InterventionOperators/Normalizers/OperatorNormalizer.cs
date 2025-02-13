using Regira.Entities.EFcore.Normalizing.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.InterventionOperators.Normalizers;

public class OperatorNormalizer(INormalizer defaultNormalizer, IdentificationNumberNormalizer idNumberNormalizer,
    ContactDataNormalizer contactDataNormalizer, AddressNormalizer addressNormalizer, IEntityNormalizer<IEntityLabel> labelNormalizer, ICultureContext cultureContext)
    : FleetEntityNormalizer<Operator>(defaultNormalizer)
{
    public override async Task HandleNormalize(Operator item)
    {
        // KBO
        item.NormalizedIdentificationNumber = idNumberNormalizer.Normalize(item.IdentificationNumber);

        // ContactData
        if (item.ContactData?.Any() == true)
        {
            await contactDataNormalizer.HandleNormalizeMany(item.ContactData);
        }

        // Address
        if (item.Addresses?.Any() == true)
        {
            foreach (var address in item.Addresses)
            {
                addressNormalizer.Normalize(address, cultureContext.LangCode);
            }
        }

        if (item.Labels?.Any() == true)
        {
            await labelNormalizer.HandleNormalizeMany(item.Labels);
        }

        // Title
        item.NormalizedTitle = DefaultPropertyNormalizer.Normalize(item.Title);

        // NormalizedContent
        var contentEntries = GetDefaultNormalizedContentEntries(item);
        contentEntries.AddRange([
            item.NormalizedIdentificationNumber?.ToUpper()
        ]);

        if (item.ContactData?.Any() == true)
        {
            contentEntries.AddRange(item.ContactData.Select(a => a.NormalizedValue));
        }

        if (item.Addresses?.Any() == true)
        {
            contentEntries.AddRange(item.Addresses.Select(a => a.NormalizedContent));
        }

        if (item.Labels?.Any() == true)
        {
            contentEntries.AddRange(item.Labels.Select(a => a.NormalizedContent));
        }

        item.NormalizedContent = string.Join(' ', contentEntries.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}