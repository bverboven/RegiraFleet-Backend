using AutoMapper;
using Regira.Fleet.Entities.Addresses;
using Regira.Fleet.Entities.Clients;
using Regira.Fleet.Entities.Clients.Subscriptions;
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
public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>()
            .ForMember(e => e.Languages, e => e.Ignore())
            .AfterMap((model, dto) =>
            {
                dto.Languages = model.Languages?.Select(l => l.LangCode).ToList();
            });
        CreateMap<ClientInputDto, Client>()
            .ForMember(e => e.Languages, e => e.Ignore())
            .AfterMap((dto, model) =>
            {
                model.Languages = dto.Languages?.Select(l => new ClientLanguage { ClientId = model.Id, LangCode = l }).ToList();
            });
        CreateMap<ClientSubscription, ClientSubscriptionDto>();
        CreateMap<ClientSubscriptionInputDto, ClientSubscription>();
    }
}