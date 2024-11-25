using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Application.Common.Security.Auth.Jwt;
using TaskManager.Application.User;

namespace TaskManager.PublicApi.Common.Extensions;

public static class JwtBearerConfigurationServiceCollectionExtensions
{
    /// <summary>
    /// Gets the secret key from environment variables, using <see cref="EnvironmentWrapper"/>,
    /// Injects the configuration from IConfiguration into <see cref="JwtAuthenticationOptions"/> and <see cref="JwtBearerOptions"/> instances
    /// </summary>
    /// <param name="services">Services</param>
    /// <param name="configuration">An instance of the IConfiguration class, where JWT authentication information comes from</param>
    /// <returns>The <see cref="IServiceCollection"/></returns>
    public static IServiceCollection ConfigureJwtAuthenticationOptionsAndAddJwtBearerAuthenticationScheme(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSecretKey = configuration["TmJwtSecretKey"] ?? throw new NullReferenceException();

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
