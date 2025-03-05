using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.Models;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.DependencyInjection.Postgres;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Globalization.LibPhoneNumber;

namespace Regira.Fleet.DependencyInjection.Entities;

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
                e.AddNormalizer<OperatorNormalizer>();
                e.AddQueryFilter<OperatorFilteredQueryBuilder>();
                if (dbType == DataBaseTypes.PostgreSQL)
                {
                    e.AddQueryFilter<OperatorFilteredPostgresLikeQueryBuilder>();
                }
                else
                {
                    e.AddQueryFilter<OperatorFilteredLikeQueryBuilder>();
                }
                e.Includes<OperatorIncludingQueryBuilder>();
                e.SortBy((query, _) => query.OrderBy(x => x.NormalizedTitle));

                e.Related(item => item.Addresses);
                e.Related(item => item.ContactData);
                e.Related(item => item.Labels);
                e.Related(item => item.Addresses);
                e.HasAttachments(item => item.Attachments);
                e.Prepare(item =>
                {
                    item.Addresses?.Prepare();
                    item.ContactData?.Prepare();
                    item.Labels?.Prepare();
                });
                e.AddPrepper<OperatorPrepper>();
            });

        return services;
    }
}