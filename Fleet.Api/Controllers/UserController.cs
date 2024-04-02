using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Api.Models;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Entities.Users.Claims;
using Regira.Fleet.Identity.Web.Extensions;
using Regira.Fleet.Identity.Web.Models;
using Regira.Serializing.Abstractions;
using Regira.Utilities;
using System.Security.Claims;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(UserManager<FleetUser> userManager, AccountsContext dbContext, ISerializer serializer, IClientContext clientContext) : ControllerBase
{
    static string[] ALLOWED_PERMISSIONS = { ClientPermissions.CanRead, ClientPermissions.CanWrite };

    [HttpPost("personal-data")]
    public async Task<IActionResult> ChangePersonalData(ChangePersonalDataInput model)
    {
        var item = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (item == null)
        {
            return NotFound();
        }

        // Culture
        if (item.Culture != model.Culture)
        {
            item.Culture = model.Culture;
            await userManager.UpdateAsync(item);
        }

        // Claims
        var claims = await userManager.GetClaimsAsync(item);
        var givenNameClaims = claims.Where(x => x.Type == FleetClaimTypes.GivenName);
        var lastNameClaims = claims.Where(x => x.Type == FleetClaimTypes.LastName);

        var claimsToRemove = new List<Claim>(givenNameClaims.Skip(1).Concat(lastNameClaims.Skip(1)));
        var claimsToAdd = new List<Claim>();

        var givenNameClaim = givenNameClaims.FirstOrDefault();
        var lastNameClaim = lastNameClaims.FirstOrDefault();

        // Given name
        if (!string.IsNullOrWhiteSpace(model.GivenName))
        {
            var claim = new Claim(FleetClaimTypes.GivenName, model.GivenName);
            if (givenNameClaim == null)
            {
                claimsToAdd.Add(claim);
            }
            else if (givenNameClaim.Value != model.GivenName)
            {
                await userManager.ReplaceClaimAsync(item, givenNameClaim, claim);
            }
        }
        else
        {
            if (givenNameClaim != null)
            {
                claimsToRemove.Add(givenNameClaim);
            }
        }
        // Last name
        if (!string.IsNullOrWhiteSpace(model.LastName))
        {
            var claim = new Claim(FleetClaimTypes.LastName, model.LastName);
            if (lastNameClaim == null)
            {
                claimsToAdd.Add(claim);
            }
            else if (lastNameClaim.Value != model.LastName)
            {
                await userManager.ReplaceClaimAsync(item, lastNameClaim, claim);
            }
        }
        else
        {
            if (lastNameClaim != null)
            {
                claimsToRemove.Add(lastNameClaim);
            }
        }

        if (claimsToRemove.Any())
        {
            await userManager.RemoveClaimsAsync(item, claimsToRemove);
        }
        if (claimsToAdd.Any())
        {
            await userManager.AddClaimsAsync(item, claimsToAdd);
        }


        return Ok();
    }



    [Authorize(FleetPolicies.AdminPolicy)]
    [HttpGet]
    public async Task<IActionResult> ListClientUsers()
    {
        var clientId = User.FindFirstValue(FleetClaimTypes.ClientId);
        var items = await dbContext.Users
            .Include(u => u.UserClaims)
            .Include(u => u.ClientClaims!.Where(x => x.ClientId == clientId))
            .Where(u => u.ClientClaims!.Any(x => x.ClientId == clientId))
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();

        var models = items
            .Select(x => new ClientUserDto
            {
                Id = x.Id,
                Email = x.Email!,
                IsEmailConfirmed = x.EmailConfirmed,
                HasPassword = !string.IsNullOrWhiteSpace(x.PasswordHash),
                DisplayName = $"{x.UserClaims!.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.GivenName)?.ClaimValue} {x.UserClaims!.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.LastName)?.ClaimValue}".Trim(),
                Permissions = x.ClientClaims!
                    .Where(x => x.ClientId == clientId)
                    .Select(c => c.ClaimValue)
                    .ToList()!
            });

        return Ok(models);
    }

    [Authorize(FleetPolicies.AdminPolicy)]
    [HttpPost]
    public async Task<IActionResult> Save(ClientUserInputDto model, [FromServices] IEmailSender mailer)
    {
        var user = await userManager.FindByNameAsync(model.Email);
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
            var token = serializer.Serialize(new UserTokenModel { Token = confirmToken, Username = user.UserName! }).Base64Encode();
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

        await SaveClientClaims(user, model.Permissions);

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

    protected async Task SaveClientClaims(FleetUser user, ICollection<string>? inputPermissions)
    {
        var currentClaims = await dbContext.ClientUserClaims
            .Where(x => x.ClientId == clientContext.ClientId && x.UserId == user.Id)
            .ToListAsync();
        var currentPermissions = currentClaims
            .Select(x => x.ClaimValue)
            .ToArray();

        var claimsToAdd = new List<ClientUserClaim>();
        if (inputPermissions != null)
        {
            foreach (var permission in inputPermissions)
            {
                if (!currentPermissions.Contains(permission))
                {
                    claimsToAdd.Add(new ClientUserClaim
                    {
                        ClientId = clientContext.ClientId!,
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
                dbContext.AddRange(claimsToAdd);
            }
            var claimsToRemove = currentClaims.Where(c => inputPermissions.All(p => p != c.ClaimValue));
            if (claimsToRemove.Any())
            {
                dbContext.RemoveRange(claimsToRemove);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
