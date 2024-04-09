using Regira.Office.Mail.Abstractions;

namespace Regira.Fleet.Identity.DependencyInjection;

public class FleetAuthenticationOptions
{
    internal Func<IServiceProvider, IMailer>? MailerFactory;
    public void AddMailer(Func<IServiceProvider, IMailer> factory)
    {
        MailerFactory = factory;
    }
}
