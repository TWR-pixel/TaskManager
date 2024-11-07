using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.User.Common.AccessToken;
using TaskManager.Application.User.Common.Security.Auth.Claims;
using TaskManager.Application.User.Common.Security.Auth.Claims.Jwt;
using TaskManager.Application.User.Common.Security.Auth.Tokens.Jwt;
using TaskManager.Application.User.Common.Security.Hashers;
using TaskManager.Application.User.Common.Security.Hashers.BCrypt;
using TaskManager.Application.User.Common.Security.SymmetricSecurityKeys;

namespace TaskManager.Application.DIExtensions;

public static class JwtAuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddScoped<IClaimsFactory, JwtClaimsFactory>();
        services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
        services.AddScoped<IKeysGenerator, KeysGenerator>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IAccessTokenFactory, AccessTokenFactory>();

        return services;
    }
}
