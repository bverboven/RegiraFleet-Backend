using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Attachments.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("intervention-operators")]
public class InterventionOperatorController : EntityControllerBase<Operator, OperatorSearchObject, EntitySortBy, OperatorIncludes, OperatorDto, OperatorInputDto>
{
}

[ApiController]
[Route("intervention-operators")]
public class InterventionOperatorAttachmentController : EntityAttachmentControllerBase<OperatorAttachment, EntityAttachmentDto, EntityAttachmentInputDto>
{
}