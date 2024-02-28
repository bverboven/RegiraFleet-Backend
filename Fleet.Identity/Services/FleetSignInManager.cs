//using System.Security.Claims;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Regira.CRM.Identity.Entities;

//namespace Regira.CRM.Identity.Services;

//public class RegiraSignInManager : SignInManager<RegiraUser>
//{
//    public RegiraSignInManager(RegiraUserManager userManager,
//        IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<RegiraUser> claimsFactory,
//        IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<RegiraUser>> logger,
//        IAuthenticationSchemeProvider schemes, IUserConfirmation<RegiraUser> userConfirmation)
//        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, userConfirmation)
//    {
//    }


//    public override bool IsSignedIn(ClaimsPrincipal principal)
//    {
//        if (principal == null)
//        {
//            throw new ArgumentNullException(nameof(principal));
//        }

//        var isAuthenticated = principal.Identities
//            .Any(i => i.AuthenticationType == IdentityConstants.ApplicationScheme
//                      || i.AuthenticationType == "AuthenticationTypes.Federation");
//        return isAuthenticated;
//    }
//}