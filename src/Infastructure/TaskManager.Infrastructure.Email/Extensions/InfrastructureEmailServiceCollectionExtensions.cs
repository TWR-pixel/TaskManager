using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Email;

namespace TaskManager.Infrastructure.Email.Extensions;

public static class InfrastructureEmailServiceCollectionExtensions
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }

    public static IServiceCollection AddEmailExistingChecker(this IServiceCollection services)
    {
        services.AddScoped<IEmailExistingChecker, EmailExistingChecker>();

        return services;
    }
}
