using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.InterventionTypes;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("intervention-types")]
public class InterventionTypeController : EntityControllerBase<InterventionType, InterventionTypeSearchObject, InterventionTypeDto, InterventionTypeInputDto>
{
}