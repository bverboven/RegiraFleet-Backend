using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Services;
using Regira.Security.Authentication.Web.Controllers;

namespace Regira.Fleet.Identity.Web.Controllers;

public class PasswordController(FleetUserIdentityManager userManager) : PasswordControllerBase<FleetUser>(userManager);
