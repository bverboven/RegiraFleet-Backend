using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Regira.Fleet.Identity.Models;

namespace Regira.Fleet.Identity.Services;

public class FleetUserManager : UserManager<FleetUser>
{
    public FleetUserManager(IUserStore<FleetUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<FleetUser> passwordHasher,
        IEnumerable<IUserValidator<FleetUser>> userValidators, IEnumerable<IPasswordValidator<FleetUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<FleetUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }
}