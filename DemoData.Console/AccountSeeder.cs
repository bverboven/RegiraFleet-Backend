using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models.Tenants;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Models.Users.Claims;
using Regira.Fleet.Identity.Services;
using Regira.Utilities;

namespace DemoData.Console;

public class AccountSeeder(FleetUserIdentityManager userManager, RoleManager<IdentityRole> roleManager, AccountsContextBase accountsContext)
{
    public async Task<IList<Tenant>> Seed()
    {
        IList<Tenant> tenants = await accountsContext.Tenants.ToListAsync();
        if (tenants.Any())
        {
            return tenants;
        }

        // SuperUser
        var superUser = new FleetUser { UserName = "admin", Email = "admin@regira.com", Culture = "nl-BE" };
        await userManager.CreateAsync(superUser, "admin");
        await roleManager.CreateAsync(new IdentityRole(FleetClaimTypes.SuperUser));
        await userManager.AddToRoleAsync(superUser, FleetClaimTypes.SuperUser);

        tenants = await SeedTenants();

        foreach (var tenant in tenants)
        {
            var permissions = new Dictionary<string, string[]>{
                    { "read" , [TenantPermissions.CanRead] },
                    { "write" , [TenantPermissions.CanRead, TenantPermissions.CanWrite] },
                    { "admin" , [TenantPermissions.Administrator, TenantPermissions.CanRead, TenantPermissions.CanWrite]
                    }
                };
            foreach (var permission in permissions)
            {
                var username = $"{tenant.Code}_{permission.Key}".ToLower();
                var user = await userManager.FindByNameAsync(username);

                if (user == null)
                {
                    user = new FleetUser
                    {
                        UserName = username,
                        Email = $"{username}@regira.com",
                        Culture = "nl-BE",
                        GivenName = tenant.Code.Capitalize(),
                        LastName = $"({permission.Key})"
                    };
                    var userResponse = await userManager.CreateAsync(user, permission.Key == "admin" ? "admin" : "demo");
                    if (userResponse.Succeeded)
                    {
                        // Identity claims
                        //await userManager.AddClaimsAsync(user, new[] {
                        //    new Claim(FleetClaimTypes.GivenName, tenant.Code.Capitalize()!),
                        //    new Claim(FleetClaimTypes.LastName, $"({permission.Key})"),
                        //});
                        // TenantUser claims
                        var userClaims = permission.Value.Select(p => new TenantUserClaim { TenantId = tenant.Id, UserId = user.Id, ClaimType = TenantClaimTypes.Permission, ClaimValue = p });
                        accountsContext.TenantUserClaims.AddRange(userClaims);
                    }
                }
            }
        }

        await accountsContext.SaveChangesAsync();

        return tenants;
    }
    public async Task<IList<Tenant>> SeedTenants()
    {
        var items = await accountsContext.Tenants.ToListAsync();
        if (!items.Any())
        {
            items.AddRange([
                new() { Code = "TRA", Title = "Openbaar vervoer", Id = "11fc2d46df234aed8df1de9a7b0f114f" },
                new() { Code = "POL", Title = "Politie", Id = "232dfc2012b8491cb7d1aaee93007480", DefaultCulture="nl-BE" },
                new() { Code = "BWR", Title = "Brandweer", Id = "1b615c0096c04eb2975ef84463aa8257", DefaultCulture="en-US" },
                new() { Code = "AMB", Title = "Ambulance", Id = "f64a75e938b64dfaae5eab03fe541972" }
            ]);
            accountsContext.Tenants.AddRange(items);
            await accountsContext.SaveChangesAsync();
        }

        return items;
    }
}