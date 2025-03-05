using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.Attachments;
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
                e.Includes((query, _) => query.Include(x => x.Translations).OrderBy(x => x.Title));
                if (dbType == DataBaseTypes.PostgreSQL)
                {
                    e.AddQueryFilter<InterventionTypePostgresLikeQueryFilter>();
                }
                else
                {
                    e.AddQueryFilter<InterventionTypeLikeQueryFilter>();
                }
                e.Related(item => item.Translations);
                e.Prepare(item => item.Translations?.Prepare());
            })
            // Intervention
            .For<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(e =>
            {
                e.AddNormalizer<InterventionNormalizer>();
                e.AddQueryFilter<InterventionQueryFilter>();
                if (dbType == DataBaseTypes.PostgreSQL)
                {
                    e.AddQueryFilter<InterventionPostgresLikeQueryFilter>();
                }
                else
                {
                    e.AddQueryFilter<InterventionLikeQueryFilter>();
                }

                e.Includes<InterventionIncludingQueryBuilder>();
                e.SortBy((query, _) => query
                    .OrderByDescending(x => x.InterventionDate ?? x.Created)
                    //.OrderByDescending(x => x.Invoices!.Max(i => i.InvoiceDate))
                    .ThenByDescending(x => x.Id));
                e.Related(item => item.Labels);
                e.Prepare(item =>
                {
                    item.Labels?.Prepare();
                });
                e.HasAttachments<TContext, Intervention, InterventionAttachment>();
                e.AddPrepper<InterventionPrepper>();
            });
        return services;
    }
}