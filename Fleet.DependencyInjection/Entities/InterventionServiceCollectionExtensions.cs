using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.Models;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
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
                e.UseWriteService<InterventionTypeWriteService>();
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
                e.AddNormalizer<FleetEntityNormalizer<InterventionType>>();
            })
            // Intervention
            .For<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(e =>
            {
                e.UseWriteService<InterventionWriteService>();
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
                e.AddNormalizer<InterventionNormalizer>();
                e.HasAttachments<TContext, Intervention, InterventionAttachment>();
            });
        return services;
    }
}