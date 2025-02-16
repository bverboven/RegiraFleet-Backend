using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.Models;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Entities.Vehicles;

public static class VehicleServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddVehicles<TContext>(this IEntityServiceCollection<TContext> services)
        where TContext : FleetContextBase
    {
        services
            // Brand
            .For<Brand, BrandSearchObject, EntitySortBy, EntityIncludes>(e =>
            {
                e.UseEntityService<BrandRepository>();
                e.AddQueryFilter<BrandQueryFilter>();
                e.AddNormalizer<FleetEntityNormalizer<Brand>>();
            })
            // VehicleType
            .For<VehicleType, VehicleTypeSearchObject, EntitySortBy, EntityIncludes>(e =>
            {
                e.UseEntityService<VehicleTypeRepository>();
                e.AddQueryFilter<VehicleQueryFilter>();
                e.AddNormalizer<FleetEntityNormalizer<VehicleType>>();
            })
            // Vehicle
            .For<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes>(e =>
            {
                e.UseEntityService<VehicleRepository>();
                e.HasRepository<VehicleRepository>();
                e.UseQueryBuilder<VehicleQueryBuilder>();
                e.AddQueryFilter<VehicleLikeFilterBuilder>();
                e.AddNormalizer<VehicleNormalizer>();
                e.HasAttachments<TContext, Vehicle, VehicleAttachment>();
            });
        return services;
    }
}