using TaskManager.PublicApi.Common.Middlewares;

namespace TaskManager.PublicApi.Common.Extensions;

public static class PublicApiServiceCollectionExtensions
{
    public static IServiceCollection AddPublicApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<HandleExceptionsMiddleware>()
            .AddScoped<IMediatorWrapper, MediatorWrapper>()
            .AddJwtBearer(configuration)
            .AddEmailSender(configuration);

        return services;
    }
}
