using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Email;

namespace TaskManager.Persistence.Email.Extensions;

public static class PersistenceEmailServiceCollectionExtensions
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
