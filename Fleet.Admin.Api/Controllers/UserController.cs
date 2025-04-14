using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Identity.Models.Users;

namespace Regira.Fleet.Admin.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController : EntityControllerBase<FleetUserModel, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes, FleetUserDto, FleetUserInputDto>;
