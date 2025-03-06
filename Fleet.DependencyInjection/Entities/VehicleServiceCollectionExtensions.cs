using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.DependencyInjection.Preppers;
using Regira.Entities.DependencyInjection.ServiceBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.DependencyInjection.Postgres;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.Normalizers;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.DependencyInjection.Entities;

public static class VehicleServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddVehicles<TContext>(this IEntityServiceCollection<TContext> services, string dbType)
        where TContext : FleetContextBase
    {
        services
            // Brand
            .For<Brand, BrandSearchObject, EntitySortBy, EntityIncludes>(e =>
            {
                e.AddNormalizer<FleetEntityNormalizer<Brand>>();
                _ = dbType == DataBaseTypes.PostgreSQL 
                    ? e.AddQueryFilter<BrandPostgresQueryFilter>() 
                    : e.AddQueryFilter<BrandQueryFilter>();
            })
            // VehicleType
            .For<VehicleType, VehicleTypeSearchObject, EntitySortBy, EntityIncludes>(e =>
            {
                e.AddNormalizer<FleetEntityNormalizer<VehicleType>>();
                _ = dbType == DataBaseTypes.PostgreSQL 
                    ? e.AddQueryFilter<VehicleTypePostgresQueryFilter>() 
                    : e.AddQueryFilter<VehicleTypeQueryFilter>();
                e.Related(item => item.Translations, item => item.Translations?.Prepare());
            })
            // Vehicle
            .For<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes>(e =>
            {
                e.AddNormalizer<VehicleNormalizer>();
                e.AddQueryFilter<VehicleFilteredQueryBuilder>();
                _ = dbType == DataBaseTypes.PostgreSQL 
                    ? e.AddQueryFilter<VehiclePostgresLikeQueryFilter>() 
                    : e.AddQueryFilter<VehicleLikeQueryFilter>();
                e.SortBy((query, _) => query.OrderBy(x => x.Code));
                e.Includes<VehicleIncludingQueryBuilder>();
                e.Related(item => item.Labels, item => item.Labels?.Prepare());
                e.AddPrepper<VehicleInterventionTypesPrepper>();
                e.HasAttachments(item => item.Attachments);
            });
        return services;
    }
}