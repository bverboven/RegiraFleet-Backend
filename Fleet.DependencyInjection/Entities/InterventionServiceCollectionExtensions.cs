using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.DependencyInjection.Preppers;
using Regira.Entities.DependencyInjection.ServiceBuilders.Abstractions;
using Regira.Entities.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.DependencyInjection.Postgres;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.Interventions.Normalizers;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.DependencyInjection.Entities;

public static class InterventionServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddInterventions<TContext>(this IEntityServiceCollection<TContext> services, string dbType)
        where TContext : FleetContextBase
    {
        services
            // InterventionType
            .For<InterventionType, InterventionTypeSearchObject, EntitySortBy, EntityIncludes>(e =>
            {
                e.AddNormalizer<FleetEntityNormalizer<InterventionType>>();
                e.AddQueryFilter<InterventionTypeQueryFilter>();
                _ = dbType == DataBaseTypes.PostgreSQL
                    ? e.AddQueryFilter<InterventionTypePostgresLikeQueryFilter>()
                    : e.AddQueryFilter<InterventionTypeLikeQueryFilter>();
                e.Includes((query, _) => query.Include(x => x.Translations).OrderBy(x => x.Title));
                e.Related(item => item.Translations, item => item.Translations?.Prepare());
            })
            // Intervention
            .For<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(e =>
            {
                e.AddNormalizer<InterventionNormalizer>();
                e.AddQueryFilter<InterventionQueryFilter>();
                _ = dbType == DataBaseTypes.PostgreSQL
                    ? e.AddQueryFilter<InterventionPostgresLikeQueryFilter>()
                    : e.AddQueryFilter<InterventionLikeQueryFilter>();
                e.Includes<InterventionIncludingQueryBuilder>();
                e.SortBy((query, _) => query
                    .OrderByDescending(x => x.InterventionDate ?? x.Created)
                    //.OrderByDescending(x => x.Invoices!.Max(i => i.InvoiceDate))
                    .ThenByDescending(x => x.Id)
                );
                e.Related(item => item.Labels, item => item.Labels?.Prepare());
                e.UseMapping<InterventionDto, InterventionInputDto>();
                e.HasAttachments(item => item.Attachments);
                e.AddPrepper<InterventionPrepper>();
            });
        return services;
    }
}