using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.Services;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Entities.InterventionTypes;

public class InterventionTypeWriteService(FleetContextBase dbContext, IEntityReadService<InterventionType, int> readService)
    : EntityWriteService<FleetContextBase, InterventionType, int>(dbContext, readService)
{
    public override async Task<InterventionType?> Modify(InterventionType item)
    {
        item.Translations?.Prepare();

        var original = await base.Modify(item);
        DbContext.UpdateEntityChildCollection(original!, item, x => x.Translations, (x, collection) => x.Translations = collection);

        return original;
    }
}