namespace TaskManager.PublicApi.Common.Middlewares;

/// <summary>
/// Middlewares for development environment
/// </summary>
public static class DevelopmentEnvironmentMiddlewareExtensions
{
    /// <summary>
    /// Enables development middlewares
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDevelopment(this IApplicationBuilder app)
    {
        Console.WriteLine("development environment");

        app.UseDeveloperExceptionPage();
        
        app.UseCors(builder => builder
             .WithOrigins(
             "http://localhost:3000",
             "https://localhost:3000",
             "https://localhost:443",
             "http://localhost:443",
             "https://localhost",
             "http://localhost",
             "https://localhost:7048")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials()
             .SetIsOriginAllowed(origin => true)
             );

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    public static IApplicationBuilder UseProduction(this IApplicationBuilder app)
    {

        return app;
    }
}
