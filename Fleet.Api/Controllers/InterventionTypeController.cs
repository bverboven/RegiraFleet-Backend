using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.Interventions.InterventionTypes;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("intervention-types")]
public class InterventionTypeController : EntityControllerBase<InterventionType, InterventionTypeSearchObject, InterventionType, InterventionTypeInputDto>
{
}