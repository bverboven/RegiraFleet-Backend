using Regira.Entities.Attachments.Models;
using Regira.Entities.Services.Abstractions;
using Regira.Entities.Validators.Abstractions;
using Regira.Fleet.Models.Abstractions;

namespace Regira.Fleet.Tenants;

/// <summary>
/// An upload names its owner in the route, and the link takes that id as it is: refuse an owner the active tenant cannot read.
/// The owner's read service applies the tenant filter, so another tenant's owner is not found.
/// </summary>
public class AttachmentOwnerValidator<TAttachment, TOwner>(IEntityReadService<TOwner, int> owners) : EntityValidatorBase<TAttachment>
    where TAttachment : EntityAttachment
    where TOwner : class, IFleetEntity
{
    public override async Task Validate(IEntityValidatorContext<TAttachment> context, CancellationToken token = default)
    {
        var item = context.Item;
        // a link that is removed, or keeps its owner, was found through the tenant filter already
        if (context.Operation == EntityWriteOperation.Remove || item.ObjectId == context.Original?.ObjectId)
        {
            return;
        }

        if (await owners.Details(item.ObjectId, token) == null)
        {
            context.AddError(nameof(EntityAttachment.ObjectId), $"{typeof(TOwner).Name} {item.ObjectId} was not found.");
        }
    }
}
