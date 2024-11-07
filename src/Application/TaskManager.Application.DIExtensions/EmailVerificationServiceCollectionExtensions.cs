using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.User.Common.Email.Messages;
using TaskManager.Application.User.Common.Email.Sender;

namespace TaskManager.Application.DIExtensions;

public static class EmailVerificationServiceCollectionExtensions
{
    public static IServiceCollection AddEmailVerification(this IServiceCollection services)
    {
        services.AddCodeVerifier();

        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IMailMessageFactory, EmailMessageFactory>();

        return services;
    }
}
