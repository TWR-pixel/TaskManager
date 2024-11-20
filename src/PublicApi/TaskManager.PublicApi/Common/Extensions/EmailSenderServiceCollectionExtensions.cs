using MailerooClient.Email.Verification;
using MailKit.Net.Smtp;
using TaskManager.Application.Common.Email;
using TaskManager.Application.User;

namespace TaskManager.PublicApi.Common.Extensions;

public static class EmailSenderServiceCollectionExtensions
{
    public static IServiceCollection ConfigureEmailSenderOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var emailApiKey = EnvironmentWrapper.GetEnvironmentVariable("TM_EMAIL_API_KEY");
        
        var emailFrom = configuration["EmailSenderOptions:EmailFrom"]!;

        if (string.IsNullOrWhiteSpace(emailFrom))
            throw new NullReferenceException("EmailFrom in configuration not found or it is empty");

        var smtpHost = configuration["EmailSenderOptions:Host"]!;

        var smtpPort = configuration.GetValue<int>("EmailSenderOptions:Port");

        services.Configure<EmailSenderOptions>(options =>
        {
            options.From = emailFrom;
            options.Host = smtpHost;
            options.Port = smtpPort;
            options.Password = emailApiKey;
            options.SmtpClient = new SmtpClient();
        });

        return services;
    }

    public static IServiceCollection ConfigureMailerooApiClientOptions(this IServiceCollection services)
    {
        var apiKey = EnvironmentWrapper.GetEnvironmentVariable("TM_MAILEROO_API_KEY");

        services.Configure<MailerooClientOptions>(options =>
        {
            options.ApiKey = apiKey;
        });

        return services;
    }
}
