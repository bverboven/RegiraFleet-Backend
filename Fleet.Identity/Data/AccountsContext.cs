using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Models;

namespace Regira.Fleet.Identity.Data;

public class AccountsContext : IdentityDbContext<FleetUser, IdentityRole, string>
{
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

        builder.Entity<FleetUser>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).HasMaxLength(2048);
            entity.Property(e => e.SecurityStamp).HasMaxLength(255);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(255);

            entity.HasMany(e => e.UserClaims)
                .WithOne()
                .HasForeignKey(e => e.UserId);
        });

        builder.Entity<IdentityRole>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(255);
            entity.Property(e => e.LoginProvider).HasMaxLength(255);
            entity.Property(e => e.ProviderKey).HasMaxLength(255);
            entity.Property(e => e.ProviderDisplayName).HasMaxLength(256);
        });

        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasMaxLength(255);
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(255);
            entity.Property(e => e.LoginProvider).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });
    }
}