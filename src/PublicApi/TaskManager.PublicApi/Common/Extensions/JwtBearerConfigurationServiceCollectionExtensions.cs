using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Application.Common.Security.Auth.Options.Jwt;
using TaskManager.PublicApi.Common.Wrappers;

namespace TaskManager.PublicApi.Common.Extensions;

public static class JwtBearerConfigurationServiceCollectionExtensions
{
    public static IServiceCollection AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
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
            options.ExpiresTokenHours = int.Parse(expiresTokenHours!);
            options.ExpiresTokenMinutes = int.Parse(expiresTokenMinutes!);
            options.SecretKey = jwtSecretKey;
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = validIssuer,
                    ValidateAudience = true,
                    ValidAudience = validAudience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                    ValidateIssuerSigningKey = true,
                };
            });


        return services;
    }
}
