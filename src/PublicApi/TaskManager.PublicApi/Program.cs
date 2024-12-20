using Microsoft.AspNetCore.HttpOverrides;
using TaskManager.PublicApi.Common.Middlewares;
using TaskManager.PublicApi.Common.Extensions;
using Serilog;
using TaskManager.Application.Common.Extensions;
using TaskManager.Infrastructure.Email.Extensions;
using TaskManager.Infrastructure.Code.Extensions;
using TaskManager.Infrastructure.Security;
using TaskManager.Persistence.Sqlite.Common.Extensions;
using TaskManager.Infrastructure.File;
using TaskManager.Application.Common;
using TaskManager.Infrastructure.Validator;
using TaskManager.Application.Common.Security.Auth.OAuth.Google;
using Microsoft.AspNetCore.Server.Kestrel.Core;

#region Configure services
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.AddSerilog();

var sqliteConnectionStr = config.GetConnectionString("Sqlite")
    ?? throw new NullReferenceException("connection string not found or empty");

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 2_000_000; // 2mbytes
});

builder.Services
    .AddPersistenceServices(sqliteConnectionStr)
    .AddApplicationServices()
    .AddPublicApiServices(config)
    .AddEmailExistingChecker()
    .AddEmailSender()
    .AddCodeGenerator()
    .AddCodeStorage()
    .AddCodeVerifier()
    .AddJwtAuthentication()
    .AddFileServices(config)
    .AddValidators()
    .AddIdentityServices();



builder.Services.Configure<GoogleOAuthOptions>(options =>
{
    options.ClientId = config["Google:ClientId"] ?? "";
    options.ClientSecret = config["Google:ClientSecret"] ?? "";
    options.ServerRedirectUri = config["Google:ServerRedirectUri"] ?? "";
    options.ClientRedirectUri = config["Google:ClientRedirectUri"] ?? "";
    options.GrantType = "authorization_code";
});

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddHttpClient(HttpClientNameConstants.Maileroo, client =>
{
    client.DefaultRequestHeaders.Add("X-API-KEY", config["TmMailerooApiKey"] ?? throw new NullReferenceException());
}).SetHandlerLifetime(TimeSpan.FromMinutes(7));

builder.Services.AddSwaggerGen();

#endregion

#region Configure middlewares
var app = builder.Build();

app.UseMiddleware<HandleExceptionsMiddleware>(); // catches all exceptions in app and logging them
app.UseSerilogRequestLogging();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseHsts();

app.UseRouting();

if (app.Environment.IsDevelopment())
    app.UseDevelopment();

else if (app.Environment.IsProduction())
    app.UseProduction();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion