using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.User.Common.Email.Code.Storage;
using TaskManager.Application.User.Common.Email.Code.Verifier;

namespace TaskManager.Application.User.Common.Email.Common.Extensions;

public static class CodeVerifierServiceCollectionExtensions
{
    public static IServiceCollection AddCodeVerifier(this IServiceCollection services)
    {
        services.AddScoped<ICodeVerifier, CodeVerifier>();
        services.AddScoped<ICodeStorage, CacheCodeStorage>();

        return services;
    }
}
