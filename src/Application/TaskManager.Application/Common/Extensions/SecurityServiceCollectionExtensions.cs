using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.User.Common.Security.AccessToken;
using TaskManager.Application.User.Common.Security.Auth;
using TaskManager.Application.User.Common.Security.Auth.Jwt.Claims;
using TaskManager.Application.User.Common.Security.Auth.Jwt.Tokens;
using TaskManager.Application.User.Common.Security.Hashers;
using TaskManager.Application.User.Common.Security.Hashers.BCrypt;
using TaskManager.Application.User.Common.Security.SymmetricSecurityKeys;

namespace TaskManager.Application.Common.Extensions;

public static class SecurityServiceCollectionExtensions
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
