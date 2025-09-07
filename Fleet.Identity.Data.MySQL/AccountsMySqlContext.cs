using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Models.Tenants;

namespace Regira.Fleet.Identity.Data.MySQL;

public class AccountsMySqlContext(DbContextOptions<AccountsMySqlContext> options) : AccountsContextBase(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TenantLanguage>(entity =>
        {
            entity.ToTable("tenant_languages");
        });
    }
}
