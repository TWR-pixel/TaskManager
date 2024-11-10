using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Application.User.Common.Security.Auth.Options.Jwt;
using TaskManager.PublicApi.Common.Wrappers;

namespace TaskManager.PublicApi.Common.Extensions;

public static class JwtBearerConfigurationServiceCollectionExtensions
{
    public static IServiceCollection ConfigureJwtAuthenticationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSecretKey = EnvironmentWrapper.GetEnvironmentVariable("TM_JWT_SECRET_KEY");

        var validIssuer = configuration["JwtAuthenticationOptions:Issuer"];
        var validAudience = configuration["JwtAuthenticationOptions:Audience"];
        var expiresTokenHours = configuration["JwtAuthenticationOptions:ExpiresTokenHours"];
        var expiresTokenMinutes = configuration["JwtAuthenticationOptions:ExpiresTokenMinutes"];

        services.Configure<JwtAuthenticationOptions>(options =>
        {
            options.Audience = validAudience ?? "localhost";
            options.Issuer = validIssuer ?? "localhost";
            options.ExpiresTokenHours = int.Parse(expiresTokenHours is null ? "12" : expiresTokenHours); // set 12 hours if expiresTokenHours is null
            options.ExpiresTokenMinutes = int.Parse(expiresTokenMinutes is null ? "12" : expiresTokenMinutes); // set 12 minutes if expiresTokenMinutes is null
            options.SecretKey = jwtSecretKey;
        });

        services.ConfigureJwtBearerAuthenticationScheme(validIssuer, validAudience, jwtSecretKey);

        return services;
    }

    public static IServiceCollection ConfigureJwtBearerAuthenticationScheme(this IServiceCollection services,
                                                                            string? issuer,
                                                                            string? audience,
                                                                            string secretKey)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }
}
