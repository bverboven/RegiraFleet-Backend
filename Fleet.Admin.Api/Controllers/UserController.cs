using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Identity.Entities.Users;

namespace Regira.Fleet.Admin.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("users")]
public class UserController : EntityControllerBase<FleetUserModel, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes, FleetUserDto, FleetUserInputDto>
{
}
