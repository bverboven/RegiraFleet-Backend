using AutoMapper;
using Regira.Fleet.Models.Translations;

namespace Regira.Fleet.Models.InterventionTypes;

public class InterventionTypeProfile : Profile
{
    public InterventionTypeProfile()
    {
        CreateMap<InterventionType, InterventionTypeDto>();
        CreateMap<InterventionTypeInputDto, InterventionType>()
            .AfterMap((_, model) =>
            {
                model.Translations = model.Translations
                    ?.Where(x => !string.IsNullOrWhiteSpace(x.Title))
                    .ToList();
            });
        CreateMap<InterventionTypeTranslation, TranslationDto>()
            .ReverseMap();
    }
}
