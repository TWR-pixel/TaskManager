using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Security;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Application.Common.Security.Auth;
using TaskManager.Application.Common.Security.Auth.Jwt;
using TaskManager.Persistence.Security.AccessToken;
using TaskManager.Persistence.Security.Hashers;
using TaskManager.Persistence.Security.Jwt;

namespace TaskManager.Persistence.Security;

public static class PersistenceSecurityServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IClaimsFactory, JwtClaimsFactory>();
        services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IAccessTokenFactory, AccessTokenFactory>();

        return services;
    }
}
