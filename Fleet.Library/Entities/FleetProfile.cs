using AutoMapper;
using Regira.Fleet.Entities.Addresses;
using Regira.Fleet.Entities.InterventionOperators.Addresses;
using Regira.Fleet.Entities.InterventionOperators.ContactData;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;

namespace Regira.Fleet.Entities;

public class FleetProfile : Profile
{
    public FleetProfile()
    {
        CreateMap<Vehicle, VehicleDto>();
        CreateMap<VehicleInputDto, Vehicle>();
        CreateMap<VehicleType, VehicleTypeDto>();
        CreateMap<VehicleTypeInputDto, VehicleType>();
        CreateMap<Brand, BrandDto>();
        CreateMap<BrandInputDto, Brand>();

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

        CreateMap<Intervention, InterventionDto>()
            .ForMember(e => e.InterventionTypes, e => e.Ignore())
            .AfterMap((model, dto, ctx) =>
            {
                if (model.InterventionTypes != null)
                {
                    dto.InterventionTypes = ctx.Mapper.Map<List<InterventionTypeDto>>(model.InterventionTypes!.Select(x => x.InterventionType));
                }
            });
        CreateMap<InterventionInputDto, Intervention>()
            .ForMember(e => e.InterventionTypes, e => e.Ignore())
            .AfterMap((dto, model) =>
            {
                if (dto.InterventionTypes != null)
                {
                    model.InterventionTypes = dto.InterventionTypes
                        .Select(x => new InterventionInterventionType
                        {
                            InterventionId = model.Id,
                            InterventionTypeId = x.Id
                        })
                        .ToList();
                }
            });
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceInputDto, Invoice>();
        CreateMap<InterventionType, InterventionTypeDto>();
        CreateMap<InterventionTypeInputDto, InterventionType>();
    }
}