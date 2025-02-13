using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Clients;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Models.Users.Claims;
using Regira.Fleet.Identity.Services;
using Regira.Utilities;

namespace DemoData.Console;

public class AccountSeeder(FleetUserIdentityManager userManager, RoleManager<IdentityRole> roleManager, AccountsContextBase accountsContext)
{
    public async Task<IList<Client>> Seed()
    {
        IList<Client> clients = await accountsContext.Clients.ToListAsync();
        if (clients.Any())
        {
            return clients;
        }

        // SuperUser
        var superUser = new FleetUser { UserName = "admin", Email = "admin@regira.com", Culture = "nl-BE" };
        await userManager.CreateAsync(superUser, "admin");
        await roleManager.CreateAsync(new IdentityRole(FleetClaimTypes.SuperUser));
        await userManager.AddToRoleAsync(superUser, FleetClaimTypes.SuperUser);

        clients = await SeedClients();

        foreach (var client in clients)
        {
            var permissions = new Dictionary<string, string[]>{
                    { "read" , [ClientPermissions.CanRead] },
                    { "write" , [ClientPermissions.CanRead, ClientPermissions.CanWrite] },
                    { "admin" , [ClientPermissions.Administrator, ClientPermissions.CanRead, ClientPermissions.CanWrite]
                    }
                };
            foreach (var permission in permissions)
            {
                var username = $"{client.Code}_{permission.Key}".ToLower();
                var user = await userManager.FindByNameAsync(username);

                if (user == null)
                {
                    user = new FleetUser
                    {
                        UserName = username,
                        Email = $"{username}@regira.com",
                        Culture = "nl-BE",
                        GivenName = client.Code.Capitalize(),
                        LastName = $"({permission.Key})"
                    };
                    var userResponse = await userManager.CreateAsync(user, permission.Key == "admin" ? "admin" : "demo");
                    if (userResponse.Succeeded)
                    {
                        // Identity claims
                        //await userManager.AddClaimsAsync(user, new[] {
                        //    new Claim(FleetClaimTypes.GivenName, client.Code.Capitalize()!),
                        //    new Claim(FleetClaimTypes.LastName, $"({permission.Key})"),
                        //});
                        // ClientUser claims
                        var userClaims = permission.Value.Select(p => new ClientUserClaim { ClientId = client.Id, UserId = user.Id, ClaimType = ClientClaimTypes.Permission, ClaimValue = p });
                        accountsContext.ClientUserClaims.AddRange(userClaims);
                    }
                }
            }
        }

        await accountsContext.SaveChangesAsync();

        return clients;
    }
    public async Task<IList<Client>> SeedClients()
    {
        var items = await accountsContext.Clients.ToListAsync();
        if (!items.Any())
        {
            items.AddRange([
                new() { Code = "TRA", Title = "Openbaar vervoer", Id = "11fc2d46df234aed8df1de9a7b0f114f" },
                new() { Code = "POL", Title = "Politie", Id = "232dfc2012b8491cb7d1aaee93007480", DefaultCulture="nl-BE" },
                new() { Code = "BWR", Title = "Brandweer", Id = "1b615c0096c04eb2975ef84463aa8257", DefaultCulture="en-US" },
                new() { Code = "AMB", Title = "Ambulance", Id = "f64a75e938b64dfaae5eab03fe541972" }
            ]);
            accountsContext.Clients.AddRange(items);
            await accountsContext.SaveChangesAsync();
        }

        return items;
    }
}