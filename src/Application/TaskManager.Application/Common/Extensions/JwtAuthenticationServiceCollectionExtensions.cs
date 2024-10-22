using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Security.Auth.Claims.Jwt;
using TaskManager.Application.Common.Security.Auth.Claims;
using TaskManager.Application.Common.Security.Auth.Tokens.Jwt;
using TaskManager.Application.Common.Security.Auth.Tokens;
using TaskManager.Application.Common.Security.Hashers.BCrypt;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Application.Common.Security.SymmetricSecurityKeys;

namespace TaskManager.Application.Common.Extensions;

public static class JwtAuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddScoped<IClaimsFactory, JwtClaimsFactory>();
        services.AddScoped<IJwtSecurityTokenFactory, JwtSecurityTokenFactory>();
        services.AddScoped<ISymmetricSecurityKeysGenerator, SymmetricSecurityKeysGenerator>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }
}
