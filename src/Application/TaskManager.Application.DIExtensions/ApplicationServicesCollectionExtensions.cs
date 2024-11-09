using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Application.DIExtensions;

public static class ApplicationServicesCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddEmailVerification()
            .AddSecurity()
            .AddMediatR();

        return services;
    }
}
