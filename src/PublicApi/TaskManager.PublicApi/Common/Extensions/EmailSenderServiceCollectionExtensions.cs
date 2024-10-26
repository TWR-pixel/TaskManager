﻿using System.Net.Mail;
using System.Net;
using TaskManager.PublicApi.Common.Wrappers;
using TaskManager.Application.Modules.Email.Options;

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

        services.Configure<EmailSenderOptions>(options =>
        {
            options.From = emailFrom;
            options.Host = smtpHost;
            options.Port = smtpPort;
            options.Password = emailApiKey;
            options.SmtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(emailFrom, emailApiKey),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        });

        return services;
    }
}