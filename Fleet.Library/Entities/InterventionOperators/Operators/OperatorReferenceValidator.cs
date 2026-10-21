using Microsoft.EntityFrameworkCore;
using Regira.Entities.Services.Abstractions;
using Regira.Entities.Validators.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.InterventionTypes;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

/// <summary>
/// An operator may only link intervention types of its own tenant. The read service applies the tenant filter (the DbContext
/// does not), so another tenant's type is not found. A link the save keeps as stored is not checked again.
/// </summary>
public class OperatorReferenceValidator(FleetContextBase dbContext, IEntityReadService<InterventionType, int> interventionTypes) : EntityValidatorBase<Operator>
{
    public override async Task Validate(IEntityValidatorContext<Operator> context, CancellationToken token = default)
    {
        var item = context.Item;
        // null = the save leaves the stored links as they are
        if (context.Operation == EntityWriteOperation.Remove || item.InterventionTypes == null)
        {
            return;
        }

        var storedTypeIds = await dbContext.Set<OperatorInterventionType>()
            .Where(x => x.OperatorId == item.Id)
            .Select(x => x.InterventionTypeId)
            .ToListAsync(token);
        foreach (var typeId in item.InterventionTypes.Select(x => x.InterventionTypeId).Except(storedTypeIds).Distinct())
        {
            if (await interventionTypes.Details(typeId, token) == null)
            {
                context.AddError(nameof(Operator.InterventionTypes), $"Intervention type {typeId} was not found.");
            }
        }
    }
}
