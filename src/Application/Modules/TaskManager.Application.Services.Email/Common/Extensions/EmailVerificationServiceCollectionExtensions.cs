using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Modules.Email.Sender;

namespace TaskManager.Application.Modules.Email.Common.Extensions;

public static class EmailVerificationServiceCollectionExtensions
{
    public static IServiceCollection AddEmailVerification(this IServiceCollection services)
    {
        services.AddCodeVerifier();

        services.AddScoped<IEmailSenderService, EmailSenderService>();

        return services;
    }
}
