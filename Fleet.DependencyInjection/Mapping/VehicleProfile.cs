using AutoMapper;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Translations;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.DependencyInjection.Mapping;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(e => e.InterventionTypes, e => e.Ignore())
            .AfterMap((model, dto, ctx) =>
            {
                if (model.InterventionTypes != null)
                {
                    dto.InterventionTypes = ctx.Mapper.Map<List<InterventionTypeDto>>(model.InterventionTypes.Select(x => x.InterventionType));
                }
            });
        CreateMap<VehicleInputDto, Vehicle>()
            .ForMember(e => e.InterventionTypes, e => e.Ignore())
            .AfterMap((dto, model) =>
            {
                if (dto.InterventionTypes != null)
                {
                    model.InterventionTypes = dto.InterventionTypes
                        .Select(x => new VehicleInterventionType
                        {
                            VehicleId = model.Id,
                            InterventionTypeId = x.Id
                        })
                        .ToList();
                }
            });

        CreateMap<VehicleType, VehicleTypeDto>();
        CreateMap<VehicleTypeInputDto, VehicleType>()
            .AfterMap((_, model) =>
            {
                model.Translations = model.Translations
                    ?.Where(x => !string.IsNullOrWhiteSpace(x.Title))
                    .ToList();
            });

        CreateMap<VehicleLabel, EntityLabelDto>();
        CreateMap<EntityLabelInputDto, VehicleLabel>();

        CreateMap<VehicleTypeTranslation, TranslationDto>()
            .ReverseMap();
        CreateMap<Brand, BrandDto>();
        CreateMap<BrandInputDto, Brand>();
    }
}
