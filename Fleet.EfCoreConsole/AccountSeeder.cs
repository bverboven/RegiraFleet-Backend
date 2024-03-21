using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Entities.Users.Claims;
using Regira.Fleet.Identity.Services;
using Regira.Utilities;
using System.Security.Claims;

namespace Fleet.EfCoreConsole;

public class AccountSeeder(FleetUserIdentityManager userManager, RoleManager<IdentityRole> roleManager, AccountsContext accountsContext)
{
    public async Task<IList<Client>> Seed()
    {
        // SuperUser
        var superUser = new FleetUser { UserName = "admin", Email = "admin@regira.com", Culture = "nl-BE" };
        await userManager.CreateAsync(superUser, "admin");
        await roleManager.CreateAsync(new IdentityRole(FleetClaimTypes.SuperUser));
        await userManager.AddToRoleAsync(superUser, FleetClaimTypes.SuperUser);

        var clients = await SeedClients();

        foreach (var client in clients)
        {
            var permissions = new Dictionary<string, string[]>{
                { "read" ,  new[] { ClientPermissions.CanRead } },
                { "write" ,  new[] { ClientPermissions.CanRead, ClientPermissions.CanWrite } },
                { "admin" ,  new[] { ClientPermissions.Administrator, ClientPermissions.CanRead, ClientPermissions.CanWrite } }
            };
            foreach (var permission in permissions)
            {
                var username = $"{client.Code}_{permission.Key}".ToLower();
                var user = await userManager.FindByNameAsync(username);

                if (user == null)
                {
                    user = new FleetUser { UserName = username, Email = $"{username}@regira.com", Culture = "nl-BE" };
                    var userResponse = await userManager.CreateAsync(user, permission.Key == "admin" ? "admin" : "demo");
                    if (userResponse.Succeeded)
                    {
                        // Identity claims
                        await userManager.AddClaimsAsync(user, new[] {
                            new Claim(FleetClaimTypes.ClientId, client.Id),
                            new Claim(FleetClaimTypes.GivenName, client.Code.Capitalize()!),
                            new Claim(FleetClaimTypes.LastName, $"({permission.Key})"),
                        });
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
            items.AddRange(new Client[]
            {
                new() { Code = "TRA", Title = "Openbaar vervoer", Id = "11fc2d46df234aed8df1de9a7b0f114f" },
                new() { Code = "POL", Title = "Politie", Id = "232dfc2012b8491cb7d1aaee93007480", DefaultCulture="nl-BE" },
                new() { Code = "BWR", Title = "Brandweer", Id = "1b615c0096c04eb2975ef84463aa8257", DefaultCulture="en-US" },
                new() { Code = "AMB", Title = "Ambulance", Id = "f64a75e938b64dfaae5eab03fe541972" }
            });
            accountsContext.Clients.AddRange(items);
            await accountsContext.SaveChangesAsync();
        }

        return items;
    }
}