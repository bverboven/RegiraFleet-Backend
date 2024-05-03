using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Models.Clients;
using Regira.Fleet.Identity.Models.Clients.Subscriptions;
using Regira.Fleet.Identity.Models.Users;
using Regira.Fleet.Identity.Models.Users.Claims;

namespace Regira.Fleet.Identity.Data;

public abstract class AccountsContextBase(DbContextOptions options) : IdentityDbContext<FleetUser, IdentityRole, string>(options), IAccountsDbContext
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ClientSubscription> ClientSubscriptions { get; set; } = null!;
    public DbSet<ClientUserClaim> ClientUserClaims { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ClientLanguage>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.LangCode });
        });
        builder.Entity<ClientUserClaim>(entity =>
        {
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.ClientId, e.UserId, e.ClaimType, e.ClaimValue })
                .IsUnique();
        });


        builder.Entity<FleetUser>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(32);
            entity.Property(e => e.PasswordHash).HasMaxLength(2048);
            entity.Property(e => e.SecurityStamp).HasMaxLength(64);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(64);

            entity.HasMany(e => e.UserClaims)
                .WithOne()
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.ClientClaims)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);
        });
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.HasIndex(e => e.UserId);
            entity.Property(e => e.ClaimType).HasMaxLength(64);
            entity.Property(e => e.ClaimValue).HasMaxLength(256);
        });

        builder.Entity<IdentityRole>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(64);
            entity.Property(e => e.LoginProvider).HasMaxLength(256);
            entity.Property(e => e.ProviderKey).HasMaxLength(256);
            entity.Property(e => e.ProviderDisplayName).HasMaxLength(256);
        });

        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(64);
            entity.Property(e => e.RoleId).HasMaxLength(64);
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(64);
            entity.Property(e => e.LoginProvider).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
        });
    }
}