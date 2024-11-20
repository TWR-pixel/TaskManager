using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Application.Common.Extensions;

public static class ApplicationServicesCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddMediatRRequestHandlers();

        return services;
    }
}
