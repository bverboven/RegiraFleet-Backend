using AutoMapper;
using Regira.Fleet.Models.Addresses;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Addresses;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.DependencyInjection.Mapping;

public class OperatorProfile : Profile
{
    public OperatorProfile()
    {
        CreateMap<OperatorAddress, AddressDto>();
        CreateMap<AddressInputDto, OperatorAddress>();

        CreateMap<OperatorContactData, OperatorContactDataDto>();
        CreateMap<OperatorContactDataInputDto, OperatorContactData>();

        CreateMap<OperatorLabel, EntityLabelDto>();
        CreateMap<EntityLabelInputDto, OperatorLabel>();

        CreateMap<OperatorInterventionType, InterventionTypeDto>();
        CreateMap<InterventionTypeInputDto, OperatorInterventionType>();

        CreateMap<Operator, OperatorDto>()
            .ForMember(e => e.InterventionTypes, e => e.Ignore())
            .AfterMap((model, dto, ctx) =>
            {
                if (model.InterventionTypes != null)
                {
                    dto.InterventionTypes = ctx.Mapper.Map<List<InterventionTypeDto>>(model.InterventionTypes.Select(x => x.InterventionType));
                }
            });
        CreateMap<OperatorInputDto, Operator>()
            .ForMember(e => e.InterventionTypes, e => e.Ignore())
            .AfterMap((dto, model, ctx) =>
            {
                if (dto.InterventionTypes != null)
                {
                    model.InterventionTypes = dto.InterventionTypes
                        .Select(x => new OperatorInterventionType
                        {
                            OperatorId = model.Id,
                            InterventionTypeId = x.Id,
                            InterventionType = ctx.Mapper.Map<InterventionType>(x)
                        })
                        .ToList();
                }
            });
    }
}
