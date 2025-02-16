using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.Models;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Globalization.LibPhoneNumber;

namespace Regira.Fleet.DependencyInjection.Entities.InterventionOperators;

public static class OperatorServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddOperators<TContext>(this IEntityServiceCollection<TContext> services, string dbType)
        where TContext : FleetContextBase
    {
        services
            .AddTransient<AddressNormalizer, AddressNormalizer>()
            .AddTransient<ContactDataNormalizer>()
            .AddTransient<IdentificationNumberNormalizer>()
            .AddTransient(p => new PhoneNumberFormatter(p.GetRequiredService<ICultureContext>().Culture));

        services
            .For<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes>(e =>
            {
                e.UseEntityService<OperatorRepository>();
                e.HasRepository<OperatorRepository>();
                e.UseQueryBuilder<OperatorQueryBuilder>();
                e.AddQueryFilter<OperatorFilteredQueryBuilder>();
                if (dbType == DataBaseTypes.PostgreSQL)
                {
                    e.AddQueryFilter<OperatorFilteredPostgresLikeQueryBuilder>();
                }
                else
                {
                    e.AddQueryFilter<OperatorFilteredLikeQueryBuilder>();
                }
                e.HasAttachments<TContext, Operator, OperatorAttachment>();
                e.AddNormalizer<OperatorNormalizer>();
            });

        return services;
    }
}