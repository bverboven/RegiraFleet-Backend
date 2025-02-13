using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.Models;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Globalization.LibPhoneNumber;

namespace Regira.Fleet.Entities.InterventionOperators;

public static class OperatorServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddOperators<TContext>(this IEntityServiceCollection<TContext> services)
        where TContext : FleetContextBase
    {
        services
            .For<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes>(e =>
            {
                e.UseEntityService<OperatorRepository>();
                e.HasRepository<OperatorRepository>();
                e.UseQueryBuilder<OperatorQueryBuilder>();
                e.HasAttachments<TContext, Operator, OperatorAttachment>();
                e
                    .AddTransient<AddressNormalizer, AddressNormalizer>()
                    .AddTransient(p => new PhoneNumberFormatter(p.GetRequiredService<ICultureContext>().Culture))
                    .AddTransient<IdentificationNumberNormalizer>()
                    .AddTransient<ContactDataNormalizer, ContactDataNormalizer>()
                    .AddNormalizer<OperatorNormalizer>();
            });
        return services;
    }
}