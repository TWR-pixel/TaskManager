using TaskManager.PublicApi.Common.Middlewares;
using TaskManager.PublicApi.Common.Wrappers.Mediator;

namespace TaskManager.PublicApi.Common.Extensions;

public static class PublicApiServiceCollectionExtensions
{
    public static IServiceCollection AddPublicApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<HandleExceptionsMiddleware>()
            .AddScoped<IMediatorWrapper, MediatorWrapper>()
            .AddJwtBearer(configuration)
            .AddEmailSender(configuration);

        return services;
    }
}
