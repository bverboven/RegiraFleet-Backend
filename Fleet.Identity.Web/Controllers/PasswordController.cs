using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Regira.Fleet.Identity.Services;
using Regira.Fleet.Identity.Web.Extensions;
using Regira.Fleet.Identity.Web.Models;
using Regira.Serializing.Abstractions;
using Regira.Utilities;

namespace Regira.Fleet.Identity.Web.Controllers;

[ApiController]
[Route("auth/password")]
public class PasswordController(FleetUserManager userManager, ISerializer serializer) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordInput model)
    {
        var username = User.Identity!.Name;
        var user = await userManager.FindByNameAsync(username!);
        if (user == null)
        {
            return NotFound();
        }

        var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            return Ok();
        }

        ModelState.AddIdentityErrors(result.Errors);
        return BadRequest(ModelState);
    }

    [AllowAnonymous]
    [HttpPost("recover")]
    public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordInput model, [FromServices] IEmailSender mailer)
    {
        var user = await userManager.FindByNameAsync(model.Username);

        if (user != null)
        {
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var token = serializer.Serialize(new ForgotPasswordModel { Token = resetToken, Username = user.UserName! }).Base64Encode();
            var resetUri = new UriBuilder(model.SiteUrl)
            {
                Query = $"?token={token}"
            };
            var subject = $"{model.SiteName} password reset".Trim().Capitalize()!;
            var body = $@"Follow link below to reset password
{resetUri.Uri}

Token: {token}
";
            await mailer.SendEmailAsync(user.Email!, subject, body);
        }

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel input)
    {
        var tokenModel = serializer.Deserialize<ForgotPasswordModel>(input.Token.Base64Decode())!;
        var user = await userManager.FindByNameAsync(tokenModel.Username);
        if (user != null)
        {
            var response = await userManager.ResetPasswordAsync(user, tokenModel.Token, input.Password);
            if (!response.Succeeded)
            {
                ModelState.AddIdentityErrors(response.Errors);
                return BadRequest(ModelState);
            }
        }

        return Ok();
    }
}