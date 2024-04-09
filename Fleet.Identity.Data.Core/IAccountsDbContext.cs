using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Models.Clients;
using Regira.Fleet.Identity.Models.Clients.Subscriptions;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Models.Users.Claims;

namespace Regira.Fleet.Identity.Data;

public interface IAccountsDbContext
{
    DbSet<FleetUser> Users { get; set; }
    DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
    DbSet<IdentityRole> Roles { get; set; }

    DbSet<Client> Clients { get; set; }
    DbSet<ClientSubscription> ClientSubscriptions { get; set; }
    DbSet<ClientUserClaim> ClientUserClaims { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}