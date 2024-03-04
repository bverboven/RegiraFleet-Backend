using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Attachments.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.Interventions.Actions;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("interventions")]
public class InterventionActionController : EntityControllerBase<InterventionAction, InterventionActionSearchObject, EntitySortBy, InterventionActionIncludes, InterventionActionDto, InterventionActionInputDto>
{
}

[ApiController]
[Route("interventions")]
public class InterventionAttachmentController : EntityAttachmentControllerBase<InterventionActionAttachment, EntityAttachmentDto, EntityAttachmentInputDto>
{
}