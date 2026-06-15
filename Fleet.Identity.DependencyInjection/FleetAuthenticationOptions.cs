using Regira.Office.Mail.Abstractions;

namespace Regira.Fleet.Identity.DependencyInjection;

public class FleetAuthenticationOptions
{
    internal Func<IServiceProvider, IMailService>? MailerFactory;
    public void AddMailer(Func<IServiceProvider, IMailService> factory)
    {
        MailerFactory = factory;
    }
}
