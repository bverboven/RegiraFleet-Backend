using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.DependencyInjection.Normalizers;
using Regira.Entities.DependencyInjection.Primers;
using Regira.Entities.DependencyInjection.QueryBuilders;
using Regira.Entities.Mapping.Mapster;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Core.DependencyInjection;
using Regira.Fleet.Core.GlobalQueryFilters;
using Regira.Fleet.Data;
using Regira.Fleet.DependencyInjection.Entities;
using Regira.Fleet.Entities.EntityLabels;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Tenants;
using Regira.IO.Storage.Abstractions;
using Regira.Normalizing.Models;
using FilterHasNormalizedContentQueryBuilder = Regira.Entities.EFcore.QueryBuilders.GlobalFilterBuilders.FilterHasNormalizedContentQueryBuilder;
using PgFilterHasNormalizedContentQueryBuilder = Regira.Fleet.Data.PostgreSQL.QueryBuilders.FilterHasNormalizedContentQueryBuilder;


namespace Regira.Fleet.DependencyInjection;

public class FleetServiceBuilder(IServiceCollection services, FleetHostingOptions options)
    : FleetServiceBuilder<FleetContextBase>(services, options)
{

    public FleetServiceBuilder AddFleetEntities(FleetHostingOptions options)
    {
        Entities = Services
            //Entity context
            .UseEntities<FleetContextBase>(c =>
            {
                c.UseMapsterMapping();
                //c.UseAutoMapper((_, o) => o.AddProfile(typeof(FleetProfile)));
                c.AddNormalizer<IEntityLabel, EntityLabelNormalizer>();
                c.UseDefaults(ed => ed.ConfigureNormalizing(o => o.Transform = TextTransform.ToUpperCase));

                // make sure only allowed tenantId items are loaded
                c.AddGlobalFilterQueryBuilder<FilterHasTenantQueryBuilder>();
                c.AddPrimer<HasTenantPrimer>();
                // ArchivablePrimer is registered by UseDefaults()

                // Postgres ILike?
                _ = options.DatabaseType == DataBaseTypes.PostgreSQL
                    ? c.AddGlobalFilterQueryBuilder<PgFilterHasNormalizedContentQueryBuilder>()
                    : c.AddGlobalFilterQueryBuilder<FilterHasNormalizedContentQueryBuilder>();

                // host-supplied options (e.g. UseAttachmentUris() from a web host)
                options.EntityOptionsFactory?.Invoke(c);
            })
            .WithAttachments(options.FileServiceFactory ?? throw new InvalidOperationException($"No implementation for {nameof(IFileService)} configured"));

        // Entity Items
        Entities
            // Countries
            .AddCountries()
            // Interventions
            .AddInterventions(options.DatabaseType)
            // Vehicles
            .AddVehicles(options.DatabaseType)
            // InterventionOperators
            .AddOperators(options.DatabaseType);

        return this;
    }
}
