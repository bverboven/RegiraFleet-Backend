using AutoMapper;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.Suppliers;
using Regira.Fleet.Entities.Suppliers.Addresses;
using Regira.Fleet.Entities.Suppliers.ContactData;

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

        CreateMap<Supplier, SupplierDto>();
        CreateMap<SupplierInputDto, Supplier>();
        CreateMap<Address, AddressDto>();
        CreateMap<AddressInputDto, Address>();
        CreateMap<SupplierContactData, SupplierContactDataDto>();
        CreateMap<SupplierContactDataInputDto, SupplierContactData>();

        CreateMap<Intervention, InterventionDto>();
        CreateMap<InterventionInputDto, Intervention>();
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceInputDto, Invoice>();
        CreateMap<InterventionType, InterventionTypeDto>();
        CreateMap<InterventionTypeInputDto, InterventionType>();
    }
}