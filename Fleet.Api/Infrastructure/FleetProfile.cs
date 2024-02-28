using AutoMapper;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Entities.Cars;
using Regira.Fleet.Entities.Cars.Brands;
using Regira.Fleet.Entities.Cars.CarTypes;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.Suppliers;
using Regira.Fleet.Entities.Suppliers.ContactData;
using Regira.Fleet.Entities.Suppliers.SupplierTypes;

namespace Regira.Fleet.Api.Infrastructure;

public class FleetProfile : Profile
{
    public FleetProfile()
    {
        CreateMap<CarInputDto, Car>();
        CreateMap<CarTypeInputDto, CarType>();
        CreateMap<BrandInputDto, Brand>();

        CreateMap<SupplierInputDto, Supplier>();
        CreateMap<SupplierTypeInputDto, SupplierType>();
        CreateMap<AddressInputDto, Address>();
        CreateMap<SupplierContactDataInputDto, SupplierContactData>();

        CreateMap<InterventionInputDto, Intervention>();
        CreateMap<InvoiceInputDto, Invoice>();
        CreateMap<InterventionTypeInputDto, InterventionType>();
    }
}