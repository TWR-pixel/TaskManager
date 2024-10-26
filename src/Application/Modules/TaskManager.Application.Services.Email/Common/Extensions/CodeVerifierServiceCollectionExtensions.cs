using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Code.Verifier;

namespace TaskManager.Application.Modules.Email.Common.Extensions;

public static class CodeVerifierServiceCollectionExtensions
{
    public static IServiceCollection AddCodeVerifier(this IServiceCollection services)
    {
        services.AddScoped<ICodeVerifier, CodeVerifier>();
        services.AddScoped<ICodeStorage, CacheCodeStorage>();

        return services;
    }
}
