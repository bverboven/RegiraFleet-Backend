using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.Clients;
using Regira.Fleet.Identity.Models;
using Regira.Fleet.Identity.Services;
using Regira.Utilities;
using System.Security.Claims;

namespace Fleet.EfCoreConsole;

public class AccountSeeder(FleetUserManager userManager, RoleManager<IdentityRole> roleManager, FleetContext fleetContext)
{
    public async Task Seed()
    {
        // SuperUser
        var superUser = new FleetUser { UserName = "admin", Email = "admin@regira.com", Culture = "nl-BE" };
        await userManager.CreateAsync(superUser, "admin");
        await roleManager.CreateAsync(new IdentityRole(FleetClaimTypes.SuperUser));
        await userManager.AddToRoleAsync(superUser, FleetClaimTypes.SuperUser);

        var clients = await fleetContext.Clients.ToListAsync();

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
                            new Claim(FleetClaimTypes.ClientId, client.Guid),
                            new Claim(ClaimTypes.GivenName, client.Code.Capitalize()!),
                            new Claim(ClaimTypes.Surname, $"({permission.Key})"),
                        });
                        // ClientUser claims
                        var userClaims = permission.Value.Select(p => new ClientUserClaim { ClientId = client.Id, UserId = user.Id, ClaimType = ClientClaimTypes.Permission, ClaimValue = p });
                        fleetContext.ClientUserClaims.AddRange(userClaims);
                    }
                }
            }
        }

        await fleetContext.SaveChangesAsync();
    }
}