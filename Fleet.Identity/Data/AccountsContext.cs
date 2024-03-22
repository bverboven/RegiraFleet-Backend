using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Entities.Clients.Subscriptions;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Entities.Users.Claims;

namespace Regira.Fleet.Identity.Data;

public class AccountsContext : IdentityDbContext<FleetUser, IdentityRole, string>
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ClientSubscription> ClientSubscriptions { get; set; } = null!;
    public DbSet<ClientUserClaim> ClientUserClaims { get; set; } = null!;

    public AccountsContext(DbContextOptions<AccountsContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ClientLanguage>(entity =>
        {
            entity.ToTable("client_languages");
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
                .WithOne()
                .HasForeignKey(e => e.UserId);
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