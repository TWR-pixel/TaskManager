using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Application.Common.Extensions;    

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRRequestHandlers(this IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });


        return services;
    }

}
