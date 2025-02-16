using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.Models;
using Regira.Fleet.Core.Normalizing;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.Interventions.Normalizers;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Entities.Interventions;

public static class InterventionServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddInterventions<TContext>(this IEntityServiceCollection<TContext> services)
        where TContext : FleetContextBase
    {
        services
            // InterventionType
            .For<InterventionType, InterventionTypeSearchObject, EntitySortBy, EntityIncludes>(e =>
            {
                e.UseEntityService<InterventionTypeRepository>();
                e.AddQueryFilter<InterventionTypeQueryFilter>();
                e.AddNormalizer<FleetEntityNormalizer<InterventionType>>();
            })
            // Intervention
            .For<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes>(e =>
            {
                e.UseEntityService<InterventionRepository>();
                e.HasRepository<InterventionRepository>();
                e.AddQueryFilter<InterventionQueryFilter>();
                e.UseQueryBuilder<InterventionQueryBuilder>();
                e.AddNormalizer<InterventionNormalizer>();
                e.HasAttachments<TContext, Intervention, InterventionAttachment>();
            });
        return services;
    }
}