using AutoMapper;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Bookings;
using Regira.Fleet.Brands;
using Regira.Fleet.Cars;
using Regira.Fleet.CarTypes;
using Regira.Fleet.InterventionTypes;
using Regira.Fleet.Suppliers;
using Regira.Fleet.SupplierTypes;

namespace Regira.Fleet.Api.Infrastructure;

public class FleetProfile : Profile
{
    public FleetProfile()
    {
        CreateMap<Brand, Brand>();
        CreateMap<CarType, CarType>();
        CreateMap<Car, Car>();
        CreateMap<InterventionType, InterventionType>();
        CreateMap<SupplierType, SupplierType>();
        CreateMap<Supplier, Supplier>();
        CreateMap<Booking, Booking>();

        CreateMap<BrandInputDto, Brand>();
        CreateMap<CarTypeInputDto, CarType>();
        CreateMap<CarInputDto, Car>();
        CreateMap<InterventionTypeInputDto, InterventionType>();
        CreateMap<SupplierTypeInputDto, SupplierType>();
        CreateMap<SupplierInputDto, Supplier>();
        CreateMap<BookingInputDto, Booking>()
            .AfterMap((dto, item) =>
            {
                item.TaxCategory = item.TaxCategory?.ToUpper();
            });
    }
}