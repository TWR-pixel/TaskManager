using TaskManager.PublicApi.Common.Wrappers;
using TaskManager.Application.Modules.Email.Options;
using MailKit.Net.Smtp;
using System.Threading;
using Microsoft.Extensions.Options;

namespace TaskManager.PublicApi.Common.Extensions;

public static class EmailSenderServiceCollectionExtensions
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        var emailApiKey = EnvironmentWrapper.GetEnvironmentVariable("EMAIL_SENDER_API_KEY");

        var emailFrom = configuration["EmailSenderOptions:EmailFrom"]!;

        if (string.IsNullOrWhiteSpace(emailFrom))
            throw new NullReferenceException("EmailFrom in configuration not found or it is empty");

        var smtpHost = configuration["EmailSenderOptions:Host"]!;

        var smtpPort = configuration.GetValue<int>("EmailSenderOptions:Port");
        var client = new SmtpClient();

        client.Connect(smtpHost, smtpPort, true);
        client.Authenticate(emailFrom, emailApiKey);

       // client.Verify(emailFrom);

        services.Configure<EmailSenderOptions>(options =>
        {
            options.From = emailFrom;
            options.Host = smtpHost;
            options.Port = smtpPort;
            options.Password = emailApiKey;
            options.SmtpClient = client;
        });

        return services;
    }
}
