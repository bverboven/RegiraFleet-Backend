using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Attachments.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Models.Interventions;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("interventions")]
public class InterventionController : EntityControllerBase<Intervention, InterventionSearchObject, InterventionSortBy, InterventionIncludes, InterventionDto, InterventionInputDto>
{
}

[ApiController]
[Route("interventions")]
public class InterventionAttachmentController : EntityAttachmentControllerBase<InterventionAttachment, EntityAttachmentDto, EntityAttachmentInputDto>
{
}