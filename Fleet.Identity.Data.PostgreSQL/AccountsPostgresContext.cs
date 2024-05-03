using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Identity.Models.Clients;

namespace Regira.Fleet.Identity.Data.PostgreSQL;

public class AccountsPostgresContext(DbContextOptions<AccountsPostgresContext> options) : AccountsContextBase(options)
{
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
        });
    }
}
