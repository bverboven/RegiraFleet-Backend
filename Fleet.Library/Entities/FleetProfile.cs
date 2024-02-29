using AutoMapper;
using Regira.Fleet.Entities.Cars;
using Regira.Fleet.Entities.Cars.Brands;
using Regira.Fleet.Entities.Cars.CarTypes;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.Suppliers;
using Regira.Fleet.Entities.Suppliers.Addresses;
using Regira.Fleet.Entities.Suppliers.ContactData;
using Regira.Fleet.Entities.Suppliers.SupplierTypes;

namespace Regira.Fleet.Entities;

public class FleetProfile : Profile
{
    public FleetProfile()
    {
        CreateMap<Car, CarDto>();
        CreateMap<CarInputDto, Car>();
        CreateMap<CarType, CarTypeDto>();
        CreateMap<CarTypeInputDto, CarType>();
        CreateMap<Brand, BrandDto>();
        CreateMap<BrandInputDto, Brand>();

        CreateMap<Supplier, SupplierDto>();
        CreateMap<SupplierInputDto, Supplier>();
        CreateMap<SupplierType, SupplierTypeDto>();
        CreateMap<SupplierTypeInputDto, SupplierType>();
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