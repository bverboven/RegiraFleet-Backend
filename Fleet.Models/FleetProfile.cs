using AutoMapper;
using Regira.Fleet.Models.Addresses;
using Regira.Fleet.Models.InterventionOperators.Addresses;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.Interventions.Invoices;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Models;

public class FleetProfile : Profile
{
    public FleetProfile()
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

        CreateMap<Intervention, InterventionDto>();
        CreateMap<InterventionInputDto, Intervention>()
            .AfterMap((dto, model) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Invoice?.InvoiceNumber) && dto.Invoice?.PriceExcl == null)
                {
                    model.Invoice = null;
                }
                if (model.Invoice != null)
                {
                    model.Invoice.InterventionId = model.Id;
                }
            });
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceInputDto, Invoice>();
        CreateMap<InterventionType, InterventionTypeDto>();
        CreateMap<InterventionTypeInputDto, InterventionType>();
    }
}