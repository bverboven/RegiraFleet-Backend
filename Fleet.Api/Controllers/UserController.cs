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
[Route("user")]
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
    [HttpPost]
    public async Task<IActionResult> Create(ClientUserInputDto model, [FromServices] IEmailSender mailer)
    {
        var tempToken = "";

        var user = await userManager.FindByNameAsync(model.Email);
        var clientClaims = new List<ClientUserClaim>();
        if (user != null)
        {
            clientClaims = await dbContext.ClientUserClaims.Where(x => x.ClientId == clientContext.ClientId && x.UserId == user.Id).ToListAsync();
        }
        else
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
            var confirmationUri = new UriBuilder(model.SiteUrl)
            {
                Query = $"?token={token}"
            };
            var body = $@"Welcome {user.UserName}, 
Please follow link below to confirm email:
{confirmationUri.Uri}

Token: {token}
";
            await mailer.SendEmailAsync(model.Email, "Welcome", body);

            tempToken = token;
        }

        var permissions = clientClaims.Select(x => x.ClaimValue).ToArray();

        var claimsToAdd = new List<ClientUserClaim>();
        if (model.Permissions?.Any() == true)
        {
            foreach (var permission in model.Permissions)
            {
                if (!permissions.Contains(permission))
                {
                    claimsToAdd.Add(new ClientUserClaim { ClientId = clientContext.ClientId!, UserId = user.Id, ClaimType = FleetClaimTypes.Permission, ClaimValue = permission });
                }
            }
            // filter claims to prevent adding unauthorized claims (like Admin)
            claimsToAdd = claimsToAdd.FindAll(c => ALLOWED_PERMISSIONS.Contains(c.ClaimValue));
            if (claimsToAdd.Any())
            {
                dbContext.AddRange(claimsToAdd);
            }
            var claimsToRemove = clientClaims.Where(c => model.Permissions.All(p => p != c.ClaimValue));
            if (claimsToRemove.Any())
            {
                dbContext.RemoveRange(claimsToRemove);
            }
            await dbContext.SaveChangesAsync();
        }

        return Ok(tempToken);
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
            if (!string.IsNullOrWhiteSpace(model.Password))
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
}
