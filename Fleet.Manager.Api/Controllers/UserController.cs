using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Models.Users.Claims;
using Regira.Fleet.Identity.Web.Extensions;
using Regira.Fleet.Identity.Web.Models;
using Regira.Fleet.Manager.Api.Models;
using Regira.Serializing.Abstractions;
using Regira.Utilities;
using System.Security.Claims;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("users")]
//public class UserController(UserManager<FleetUser> userManager, AccountsContextBase dbContext, ISerializer serializer, ITenantContext tenantContext) : ControllerBase
public class UserController(UserManager<FleetUser> userManager, IAccountsDbContext dbContext, ISerializer serializer, ITenantContext tenantContext) : ControllerBase
{
    static readonly string[] ALLOWED_PERMISSIONS = [TenantPermissions.CanRead, TenantPermissions.CanWrite];

    [HttpPost("personal-data")]
    public async Task<IActionResult> ChangePersonalData(ChangePersonalDataInput model)
    {
        var item = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (item == null)
        {
            return NotFound();
        }

        item.GivenName = model.GivenName;
        item.LastName = model.LastName;
        item.Culture = model.Culture;

        await userManager.UpdateAsync(item);

        return Ok();
    }



    [Authorize(FleetPolicies.AdminPolicy)]
    [HttpGet]
    public async Task<IActionResult> ListTenantUsers()
    {
        var tenantId = User.FindFirstValue(FleetClaimTypes.TenantId);
        var items = await dbContext.Users
            .Include(u => u.UserClaims)
            .Include(u => u.TenantClaims!.Where(x => x.TenantId == tenantId))
            .Where(u => u.TenantClaims!.Any(x => x.TenantId == tenantId))
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();

        var models = items
            .Select(x => new TenantUserDto
            {
                Id = x.Id,
                Email = x.Email!,
                IsEmailConfirmed = x.EmailConfirmed,
                HasPassword = !string.IsNullOrWhiteSpace(x.PasswordHash),
                DisplayName = $"{x.GivenName} {x.LastName}".Trim(),
                Permissions = x.TenantClaims!
                    .Where(claim => claim.TenantId == tenantId)
                    .Select(c => c.ClaimValue)
                    .ToList()!
            });

        return Ok(models);
    }

    [Authorize(FleetPolicies.AdminPolicy)]
    [HttpPost]
    public async Task<IActionResult> Save(TenantUserInputDto model, [FromServices] IEmailSender mailer)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            user = new FleetUser { UserName = model.Email, Email = model.Email, Culture = model.Culture };
            var response = await userManager.CreateAsync(user);
            if (!response.Succeeded)
            {
                // error
                ModelState.AddIdentityErrors(response.Errors);
                return BadRequest(ModelState);
            }
            var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var token = serializer.Serialize(new UserTokenModel { Token = confirmToken, Username = user.UserName }).Base64Encode();
            if (string.IsNullOrWhiteSpace(model.SiteUrl))
            {
                ModelState.AddModelError(nameof(model.SiteUrl), "Required for new user");
                return BadRequest(ModelState);
            }
            var confirmationUri = new UriBuilder(model.SiteUrl)
            {
                Query = $"?token={token}"
            };
            var body = $@"Welcome {user.UserName}, 
Please follow link below to confirm email:
{confirmationUri.Uri}

Token: {token}
";
            await mailer.SendEmailAsync(model.Email, "Welcome at Regira Fleetmanager", body);
        }

        await SaveTenantClaims(user, model.Permissions);

        return Ok();
    }


    [AllowAnonymous]
    [HttpPost("confirm-email", Name = RouteNames.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailInput model)
    {
        var tokenModel = serializer.Deserialize<UserTokenModel>(model.Token.Base64Decode())!;
        var user = await userManager.FindByNameAsync(tokenModel.Username);
        if (user != null)
        {
            // Confirm email
            var emailResponse = await userManager.ConfirmEmailAsync(user, tokenModel.Token);
            if (!emailResponse.Succeeded)
            {
                ModelState.AddIdentityErrors(emailResponse.Errors);
                return BadRequest(ModelState);
            }
            // Add password
            if (string.IsNullOrWhiteSpace(user.PasswordHash) && !string.IsNullOrWhiteSpace(model.Password))
            {
                var pwdResponse = await userManager.AddPasswordAsync(user, model.Password);
                if (!pwdResponse.Succeeded)
                {
                    ModelState.AddIdentityErrors(pwdResponse.Errors);
                    return BadRequest(ModelState);
                }
            }
        }
        return Ok();
    }

    [HttpPost("send-confirm-email")]
    public async Task<IActionResult> RequestConfirmEmail(RequestConfirmEmailInput input, [FromServices] IEmailSender mailer)
    {
        var user = await userManager.FindByNameAsync(input.Email);
        if (user != null)
        {
            var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var token = serializer.Serialize(new UserTokenModel { Token = confirmToken, Username = user.UserName! }).Base64Encode();

            var confirmationUri = new UriBuilder(input.SiteUrl)
            {
                Query = $"?token={token}"
            };
            var body = $@"Welcome {user.UserName}, 
Please follow link below to confirm email:
{confirmationUri.Uri}

Token: {token}
";
            await mailer.SendEmailAsync(input.Email, "Please confirm your email address", body);
        }
        return Ok();
    }

    protected async Task SaveTenantClaims(FleetUser user, ICollection<string>? inputPermissions)
    {
        var currentClaims = await dbContext.TenantUserClaims
            .Where(x => x.TenantId == tenantContext.TenantId && x.UserId == user.Id)
            .ToListAsync();
        var currentPermissions = currentClaims
            .Select(x => x.ClaimValue)
            .ToArray();

        var claimsToAdd = new List<TenantUserClaim>();
        if (inputPermissions != null)
        {
            foreach (var permission in inputPermissions)
            {
                if (!currentPermissions.Contains(permission))
                {
                    claimsToAdd.Add(new TenantUserClaim
                    {
                        TenantId = tenantContext.TenantId!,
                        UserId = user.Id,
                        ClaimType = FleetClaimTypes.Permission,
                        ClaimValue = permission
                    });
                }
            }
            // filter claims to prevent adding unauthorized claims (like Admin)
            claimsToAdd = claimsToAdd.FindAll(c => ALLOWED_PERMISSIONS.Contains(c.ClaimValue));
            if (claimsToAdd.Any())
            {
                dbContext.TenantUserClaims.AddRange(claimsToAdd);
            }
            var claimsToRemove = currentClaims
                .Where(c => inputPermissions.All(p => p != c.ClaimValue))
                .ToArray();
            if (claimsToRemove.Any())
            {
                dbContext.TenantUserClaims.RemoveRange(claimsToRemove);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
