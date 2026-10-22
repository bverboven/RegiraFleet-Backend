using Regira.Entities.Services.Abstractions;
using Regira.Entities.Validators.Abstractions;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Entities.Interventions;

/// <summary>
/// An intervention may only point at a vehicle, operator and intervention type of its own tenant. The read services apply the
/// tenant filter (the DbContext does not), so another tenant's row is not found. A reference the save keeps as stored is not checked again.
/// </summary>
public class InterventionReferenceValidator(IEntityReadService<Vehicle, int> vehicles, IEntityReadService<Operator, int> operators,
    IEntityReadService<InterventionType, int> interventionTypes) : EntityValidatorBase<Intervention>
{
    public override async Task Validate(IEntityValidatorContext<Intervention> context, CancellationToken token = default)
    {
        if (context.Operation == EntityWriteOperation.Remove)
        {
            return;
        }

        var item = context.Item;
        var original = context.Original;
        if (item.VehicleId != original?.VehicleId && await vehicles.Details(item.VehicleId, token) == null)
        {
            context.AddError(nameof(Intervention.VehicleId), $"Vehicle {item.VehicleId} was not found.");
        }
        if (item.OperatorId != original?.OperatorId && await operators.Details(item.OperatorId, token) == null)
        {
            context.AddError(nameof(Intervention.OperatorId), $"Operator {item.OperatorId} was not found.");
        }
        if (item.InterventionTypeId.HasValue && item.InterventionTypeId != original?.InterventionTypeId
            && await interventionTypes.Details(item.InterventionTypeId.Value, token) == null)
        {
            context.AddError(nameof(Intervention.InterventionTypeId), $"Intervention type {item.InterventionTypeId} was not found.");
        }
    }
}
