using Microsoft.AspNetCore.HttpOverrides;
using TaskManager.PublicApi.Common.Middlewares;
using TaskManager.PublicApi.Common.Extensions;
using Serilog;
using TaskManager.Application.User;
using TaskManager.PublicApi.Controllers;
using TaskManager.Application.Common.Extensions;
using TaskManager.DALImplementation.Sqlite.Common.Extensions;
using TaskManager.Persistence.Email.Extensions;
using TaskManager.Persistence.Code.Extensions;
using TaskManager.Persistence.Security;

#region Configure services
var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog();

var sqliteConnectionStr = builder.Configuration.GetConnectionString("Sqlite")
    ?? throw new NullReferenceException("connection string not found or empty");

builder.Services
    .AddInfrastructureServices(sqliteConnectionStr)
    .AddApplicationServices()
    .AddPublicApiServices(builder.Configuration)
    .AddEmailExistingChecker()
    .AddEmailSender()
    .AddCodeGenerator()
    .AddCodeStorage()
    .AddCodeVerifier()
    .AddJwtAuthentication();

builder.Services.AddMemoryCache();
builder.Services.AddControllers().AddApplicationPart(typeof(RoleController).Assembly);
builder.Services.AddCors();
builder.Services.AddHttpClient("Maileroo", client =>
{
    client.DefaultRequestHeaders.Add("X-API-KEY", EnvironmentWrapper.GetEnvironmentVariable("TM_MAILEROO_API_KEY"));
}).SetHandlerLifetime(TimeSpan.FromMinutes(5));

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