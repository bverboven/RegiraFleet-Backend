using AutoMapper;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.InterventionOperators.Addresses;
using Regira.Fleet.Entities.InterventionOperators.ContactData;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Interventions.Actions;
using Regira.Fleet.Entities.InterventionOperators.Operators;

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

        CreateMap<InterventionOperator, InterventionOperatorDto>();
        CreateMap<InterventionOperatorInputDto, InterventionOperator>();
        CreateMap<Address, AddressDto>();
        CreateMap<AddressInputDto, Address>();
        CreateMap<InterventionOperatorContactData, InterventionOperatorContactDataDto>();
        CreateMap<InterventionOperatorContactDataInputDto, InterventionOperatorContactData>();

        CreateMap<InterventionAction, InterventionActionDto>();
        CreateMap<InterventionActionInputDto, InterventionAction>();
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceInputDto, Invoice>();
        CreateMap<InterventionType, InterventionTypeDto>();
        CreateMap<InterventionTypeInputDto, InterventionType>();
    }
}