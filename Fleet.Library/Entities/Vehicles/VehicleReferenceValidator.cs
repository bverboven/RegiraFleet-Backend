using Microsoft.EntityFrameworkCore;
using Regira.Entities.Services.Abstractions;
using Regira.Entities.Validators.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Entities.Vehicles;

/// <summary>
/// A vehicle may only point at a brand, vehicle type and intervention types of its own tenant. The read services apply the
/// tenant filter (the DbContext does not), so another tenant's row is not found. A reference the save keeps as stored is not checked again.
/// </summary>
public class VehicleReferenceValidator(FleetContextBase dbContext, IEntityReadService<Brand, int> brands, IEntityReadService<VehicleType, int> vehicleTypes,
    IEntityReadService<InterventionType, int> interventionTypes) : EntityValidatorBase<Vehicle>
{
    public override async Task Validate(IEntityValidatorContext<Vehicle> context, CancellationToken token = default)
    {
        if (context.Operation == EntityWriteOperation.Remove)
        {
            return;
        }

        var item = context.Item;
        var original = context.Original;
        if (item.BrandId.HasValue && item.BrandId != original?.BrandId && await brands.Details(item.BrandId.Value, token) == null)
        {
            context.AddError(nameof(Vehicle.BrandId), $"Brand {item.BrandId} was not found.");
        }
        if (item.VehicleTypeId.HasValue && item.VehicleTypeId != original?.VehicleTypeId && await vehicleTypes.Details(item.VehicleTypeId.Value, token) == null)
        {
            context.AddError(nameof(Vehicle.VehicleTypeId), $"Vehicle type {item.VehicleTypeId} was not found.");
        }

        // null = the save leaves the stored links as they are
        if (item.InterventionTypes == null)
        {
            return;
        }
        var storedTypeIds = await dbContext.Set<VehicleInterventionType>()
            .Where(x => x.VehicleId == item.Id)
            .Select(x => x.InterventionTypeId)
            .ToListAsync(token);
        foreach (var typeId in item.InterventionTypes.Select(x => x.InterventionTypeId).Except(storedTypeIds).Distinct())
        {
            if (await interventionTypes.Details(typeId, token) == null)
            {
                context.AddError(nameof(Vehicle.InterventionTypes), $"Intervention type {typeId} was not found.");
            }
        }
    }
}
