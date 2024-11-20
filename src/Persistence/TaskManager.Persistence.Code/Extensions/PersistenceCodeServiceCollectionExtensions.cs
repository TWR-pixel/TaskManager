using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Code;

namespace TaskManager.Persistence.Code.Extensions;

public static class PersistenceCodeServiceCollectionExtensions
{
    public static IServiceCollection AddCodeVerifier(this IServiceCollection services)
    {
        services.AddScoped<ICodeVerifier, CodeVerifier>();

        return services;
    }

    public static IServiceCollection AddCodeGenerator(this IServiceCollection services)
    {
        services.AddSingleton<ICodeGenerator<string>, HexStringCodeGenerator>();

        return services;
    }

    public static IServiceCollection AddCodeStorage(this IServiceCollection services)
    {
        services.AddScoped<ICodeStorage, MemoryCacheCodeStorage>();

        return services;
    }
}
