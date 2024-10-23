using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Modules.Email.Common.Extensions;

namespace TaskManager.Application.Common.Extensions;

public static class ApplicationServicesCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddEmailVerification()
            .AddSecurity()
            .AddMediator();

        return services;
    }
}
