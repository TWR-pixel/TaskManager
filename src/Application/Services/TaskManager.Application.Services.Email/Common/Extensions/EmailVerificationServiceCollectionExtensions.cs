using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Message;
using TaskManager.Application.Modules.Email.Sender;
using TaskManager.Application.Modules.Email.Verifier;

namespace TaskManager.Application.Modules.Email.Common.Extensions;

public static class EmailVerificationServiceCollectionExtensions
{
    public static IServiceCollection AddEmailVerification(this IServiceCollection services)
    {
        services.AddScoped<ICodeVerifier, CodeVerifier>();
        services.AddScoped<IEmailSender, EmailSender>();

        services.AddScoped<ICodeStorage, CacheCodeStorage>();
        services.AddScoped<IVerificationMessageFactory, VerificationMessageFactory>();
        services.AddScoped<IRecoveryPasswordMessageFactory, RecoveryPasswordMessageFactory>();
        
        return services;
    }
}
