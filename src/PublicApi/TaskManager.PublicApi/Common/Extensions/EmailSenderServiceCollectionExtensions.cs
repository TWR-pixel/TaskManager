using System.Net.Mail;
using System.Net;
using TaskManager.Application.Modules.Email.Sender.Options;
using TaskManager.PublicApi.Common.Wrappers;

namespace TaskManager.PublicApi.Common.Extensions;

public static class EmailSenderServiceCollectionExtensions
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        var emailApiKey = EnvironmentWrapper.GetEnvironmentVariable("EMAIL_SENDER_API_KEY");

        var emailFrom = configuration["EmailSenderOptions:EmailFrom"]!;
        var smtpHost = configuration["EmailSenderOptions:Host"]!;

        var smtpPort = configuration.GetValue<int>("EmailSenderOptions:Port");

        services.Configure<EmailSenderOptions>(options =>
        {
            options.From = emailFrom;
            options.Host = smtpHost;
            options.Port = smtpPort;
            options.Password = emailApiKey;
            options.SmtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(emailFrom, emailApiKey),
            };
        });

        return services;
    }
}
