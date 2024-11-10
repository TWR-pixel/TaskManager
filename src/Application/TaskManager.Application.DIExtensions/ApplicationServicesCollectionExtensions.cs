using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Application.DIExtensions;

public static class ApplicationServicesCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddEmailVerification()
            .AddSecurity()
            .AddMediatRRequestHandlers();

        return services;
    }
}
