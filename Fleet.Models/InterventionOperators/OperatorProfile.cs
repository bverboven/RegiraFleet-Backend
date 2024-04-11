using AutoMapper;
using Regira.Fleet.Models.Addresses;
using Regira.Fleet.Models.InterventionOperators.Addresses;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Models.InterventionOperators;

public class OperatorProfile : Profile
{
    public OperatorProfile()
    {
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
            .AfterMap((dto, model) =>
            {
                if (dto.InterventionTypes != null)
                {
                    model.InterventionTypes = dto.InterventionTypes
                        .Select(x => new OperatorInterventionType
                        {
                            OperatorId = model.Id,
                            InterventionTypeId = x.Id
                        })
                        .ToList();
                }
            });
        CreateMap<OperatorAddress, AddressDto>();
        CreateMap<AddressInputDto, OperatorAddress>();
        CreateMap<OperatorContactData, OperatorContactDataDto>();
        CreateMap<OperatorContactDataInputDto, OperatorContactData>();
    }
}
