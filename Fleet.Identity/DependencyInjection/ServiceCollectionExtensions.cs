using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Regira.Fleet.Identity.Data;
using Regira.Fleet.Identity.Models;
using Regira.Fleet.Identity.Services;
using Regira.Office.Mail.Abstractions;
using Regira.Security.Authentication.Mail;

namespace Regira.Fleet.Identity.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public class RegiraAuthenticateOptions
    {
        internal Func<IServiceProvider, IMailer>? MailerFactory;
        public void AddMailer(Func<IServiceProvider, IMailer> factory)
        {
            MailerFactory = factory;
        }
    }

    public static IdentityBuilder AddRegiraAuthentication(this IServiceCollection services, RegiraAuthenticateOptions options)
    {
        var builder = services
            .AddIdentityCore<FleetUser>(o =>
            {
                o.SignIn.RequireConfirmedAccount = false;
                o.User.RequireUniqueEmail = true;
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredUniqueChars = 1;
                o.Password.RequiredLength = 2;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                o.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AccountsContext>()
            .AddUserManager<FleetUserManager>()
            .AddClaimsPrincipalFactory<FleetUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        if (options.MailerFactory != null)
        {
            services.AddTransient(options.MailerFactory);
            services.AddTransient<IEmailSender, IdentityMailer>();
        }

        return builder;
    }
    public static IdentityBuilder AddRegiraAuthentication(this IServiceCollection services, Action<RegiraAuthenticateOptions>? configure = null)
    {
        var options = new RegiraAuthenticateOptions();
        configure?.Invoke(options);

        return AddRegiraAuthentication(services, options);
    }
}