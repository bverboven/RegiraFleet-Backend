using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Regira.Fleet.Identity.Entities.Users;

namespace Regira.Fleet.Identity.Services;

public class FleetUserIdentityManager(IUserStore<FleetUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<FleetUser> passwordHasher,
        IEnumerable<IUserValidator<FleetUser>> userValidators, IEnumerable<IPasswordValidator<FleetUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<FleetUser>> logger)
    : UserManager<FleetUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
{
}