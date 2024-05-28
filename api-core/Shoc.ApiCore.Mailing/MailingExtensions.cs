using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Mailing;
using Shoc.Mailing.Model;
using Shoc.Mailing.Smtp;

namespace Shoc.ApiCore.Mailing;

/// <summary>
/// The mailing configuration of system
/// </summary>
public static class MailingExtensions
{
    /// <summary>
    /// Configure data environment
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="configuration">The configuration instance</param>
    public static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration configuration)
    {
        // get settings from configuration
        var settings = configuration.BindAs<MailingSettings>("Mailing");

        // register settings for future use
        services.AddSingleton(settings);

        // register SMTP email sender as default sender
        services.AddSingleton<IEmailSender, SmtpEmailSender>();

        // chain services
        return services;
    }
}
