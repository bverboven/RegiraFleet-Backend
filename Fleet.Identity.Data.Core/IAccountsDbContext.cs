using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Models.Tenants;
using Regira.Fleet.Identity.Models.Tenants.Subscriptions;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Models.Users.Claims;

namespace Regira.Fleet.Identity.Data;

public interface IAccountsDbContext
{
    DbSet<FleetUser> Users { get; set; }
    DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
    DbSet<IdentityRole> Roles { get; set; }

    DbSet<Tenant> Tenants { get; set; }
    DbSet<TenantSubscription> TenantSubscriptions { get; set; }
    DbSet<TenantUserClaim> TenantUserClaims { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}