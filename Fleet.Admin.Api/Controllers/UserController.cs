using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Services;

namespace Regira.Fleet.Admin.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("users")]
public class UserController(AccountsContext dbContext, FleetUserIdentityManager userManager) : EntityControllerBase<FleetUserModel, string, FleetUserSearchObject, EntitySortBy, FleetUserIncludes, FleetUserDto, FleetUserInputDto>
{
}
