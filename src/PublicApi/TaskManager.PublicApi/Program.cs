using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManager.Application.Common.Extensions;
using TaskManager.PublicApi.Common.Middlewares;
using TaskManager.Infastructure.Sqlite.Common.Extensions;
using TaskManager.PublicApi.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

var sqliteConnectionStr = builder.Configuration.GetConnectionString("Sqlite")
    ?? throw new NullReferenceException("connection string not found or empty");

builder.Services
    .AppInfastructure(sqliteConnectionStr)
    .AddApplication()
    .AddPublicApi(builder.Configuration);

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddCors();

#region swaggerGen configuration
builder.Services.AddSwaggerGen(c =>
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
#endregion

var app = builder.Build();

app.UseMiddleware<HandleExceptionsMiddleware>(); // catches all exceptions in app and logging them

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseHsts();

app.UseRouting();

if (app.Environment.IsDevelopment())
    app.UseDevelopment();

if (app.Environment.IsProduction())
    app.UseProduction();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();