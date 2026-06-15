using Microsoft.Extensions.DependencyInjection;
using Regira.Entities.DependencyInjection.Extensions;
using Regira.Entities.DependencyInjection.QueryBuilders;
using Regira.Entities.EFcore.QueryBuilders.GlobalFilterBuilders;
using Regira.Entities.Mapping.Mapster;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Core.DependencyInjection;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.DependencyInjection.Entities;
using Regira.Normalizing.Models;


namespace Regira.Fleet.Identity.DependencyInjection;

public class FleetServiceBuilder(IServiceCollection services, FleetHostingOptions options)
    : FleetServiceBuilder<AccountsContextBase>(services, options)
{
    public FleetServiceBuilder AddFleetEntities(FleetHostingOptions options)
    {
        Entities = Services
            // Entity context
            .UseEntities<AccountsContextBase>(c =>
            {
                c.UseMapsterMapping();
                //c.UseAutoMapper((_, o) => o.AddProfile(typeof(IdentityProfile)));
                c.UseDefaults(ed => ed.ConfigureNormalizing(o => o.Transform = TextTransform.ToUpperCase));

                c.AddGlobalFilterQueryBuilder<FilterIdsQueryBuilder<string>>();
                if (options.DatabaseType == DataBaseTypes.PostgreSQL)
                {
                    c.AddGlobalFilterQueryBuilder<Data.PostgreSQL.QueryBuilders.FilterHasNormalizedContentQueryBuilder>();
                }
                else
                {
                    c.AddGlobalFilterQueryBuilder<FilterHasNormalizedContentQueryBuilder>();
                }
            });


        Entities
            // Entity context
            .AddTenants()
            .AddFleetUsers(options.DatabaseType);

        return this;
    }
}
