using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.Services;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Entities.Vehicles.VehicleTypes;

public class VehicleTypeWriteService(FleetContextBase dbContext, IEntityReadService<VehicleType, int> readService)
    : EntityWriteService<FleetContextBase, VehicleType, int>(dbContext, readService)
{
    public override async Task<VehicleType?> Modify(VehicleType item)
    {
        item.Translations?.Prepare();
        var original = await base.Modify(item);
        DbContext.UpdateEntityChildCollection(original!, item, x => x.Translations, (x, collection) => x.Translations = collection);

        return original;
    }
}