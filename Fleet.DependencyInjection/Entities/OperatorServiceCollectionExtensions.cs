using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.DependencyInjection.ServiceBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.DependencyInjection.Postgres;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Models.Addresses;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Addresses;
using Regira.Fleet.Models.InterventionOperators.ContactData;
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
                e.AddFilter<OperatorFilteredQueryBuilder>();
                _ = dbType == DataBaseTypes.PostgreSQL
                    ? e.AddFilter<OperatorFilteredPostgresLikeQueryBuilder>()
                    : e.AddFilter<OperatorFilteredLikeQueryBuilder>();
                e.AddIncludes<OperatorIncludingQueryBuilder>();
                e.SortBy((query, _) => query.OrderBy(x => x.NormalizedTitle));

                e.Related(item => item.Addresses, item => item.Addresses?.Prepare());
                e.Related(item => item.ContactData, item => item.ContactData?.Prepare());
                e.Related(item => item.Labels, item => item.Labels?.Prepare());
                e.Related(item => item.InterventionTypes, item => item.InterventionTypes?.Prepare());
                e.HasAttachments(item => item.Attachments);

                //e.UseMapping<OperatorDto, OperatorInputDto>();
                //e.AddMapping<OperatorAddress, AddressDto>();
                //e.AddMapping<OperatorContactData, OperatorContactDataDto>();
                //e.AddMapping<OperatorLabel, EntityLabelDto>();

                e.AddNormalizer<OperatorNormalizer>();
            });

        return services;
    }
}