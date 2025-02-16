using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Fleet.Entities.Countries;
using Regira.Fleet.Models.Countries;

namespace Regira.Fleet.DependencyInjection.Entities.Countries;

public static class CountryServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddCountries<TContext>(this IEntityServiceCollection<TContext> services)
        where TContext : DbContext
    {
        services
            .For<Country, string>(e =>
            {
                e.UseEntityService<CountryRepository>();
                e.AddMapping<CountryDto, CountryDto>();
            });
        return services;
    }
}