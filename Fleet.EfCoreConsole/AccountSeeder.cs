using Regira.Fleet.Entities.Clients;
using Regira.Fleet.Identity.Constants;
using Regira.Fleet.Identity.Models;
using Regira.Fleet.Identity.Services;
using System.Security.Claims;

namespace Fleet.EfCoreConsole;

public class AccountSeeder(FleetUserManager userManager)
{
    public async Task Seed(IList<Client> clients)
    {
        foreach (var client in clients)
        {
            var username = $"fleet_{client.Code}".ToLower();
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                user = new FleetUser { UserName = username, Email = $"{username}@regira.com", Culture = "nl-BE" };
                var userResponse = await userManager.CreateAsync(user, "demo");
                if (userResponse.Succeeded)
                {
                    await userManager.AddClaimsAsync(user, new[]
                    {
                        new Claim(FleetClaimTypes.ClientId, client.Guid)
                    });
                }
            }
        }
    }
}
