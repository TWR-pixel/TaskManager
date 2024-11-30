using Microsoft.OpenApi.Models;
using Serilog;
using TaskManager.PublicApi.Common.Middlewares;

namespace TaskManager.PublicApi.Common.Extensions;

public static class PublicApiServiceCollectionExtensions
{

    /// <summary>
    /// Configure options for authentication and email sending. 
    /// Adds exceptions handler and mediator wrapper
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddPublicApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddExceptionsHandler()
            .AddMediatorWrapper();

        services
            .ConfigureJwtAuthenticationOptionsAndAddJwtBearerAuthenticationScheme(configuration);

        services
            .ConfigureEmailSenderOptions(configuration)
            .ConfigureMailerooApiClientOptions(configuration);

        return services;
    }

    public static IServiceCollection AddMediatorWrapper(this IServiceCollection services)
    {
        services.AddScoped<IMediatorWrapper, MediatorWrapper>();

        return services;
    }

    public static IServiceCollection AddGoogleIdentity(this IServiceCollection services)
    {
        

        return services;
    }

    /// <summary>
    /// Adds Transient <see cref="HandleExceptionsMiddleware"/>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddExceptionsHandler(this IServiceCollection services)
    {
        services.AddTransient<HandleExceptionsMiddleware>();

        return services;
    }

    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders(); // clear default asp .net core logging

        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Logging.AddSerilog();
        //builder.Logging.SetMinimumLevel(LogLevel.Information);

        builder.Host.UseSerilog((hostringContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(hostringContext.Configuration);
        });

        return builder;
    }

    public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "My API for task manager project",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
               Reference = new OpenApiReference
               {
                 Type = ReferenceType.SecurityScheme,
                 Id = "Bearer"
               }
             },
              Array.Empty<string>()
            }
          });
        });

        return services;
    }
}
