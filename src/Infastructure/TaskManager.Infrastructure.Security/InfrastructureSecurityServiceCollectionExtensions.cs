using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Security;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Application.Common.Security.Auth;
using TaskManager.Application.Common.Security.Auth.Jwt;
using TaskManager.Infrastructure.Security.AccessToken;
using TaskManager.Infrastructure.Security.Hashers;
using TaskManager.Infrastructure.Security.Jwt;

namespace TaskManager.Infrastructure.Security;

public static class InfrastructureSecurityServiceCollectionExtensions
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
