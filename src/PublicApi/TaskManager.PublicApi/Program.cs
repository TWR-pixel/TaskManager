using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManager.Application.Common.Extensions;
using TaskManager.PublicApi.Common.Middlewares;
using TaskManager.Infastructure.Sqlite.Common.Extensions;
using TaskManager.PublicApi.Common.Extensions;
using Serilog;
using Serilog.Context;
using System.Net.Http.Headers;
using System.Text;
using TaskManager.PublicApi.Common.Wrappers;

var builder = WebApplication.CreateBuilder(args);

//var client = new HttpClient
//{
//    BaseAddress = new Uri("https://go2.unisender.ru/ru/transactional/api/v1/")
//};

//var apiKey = EnvironmentWrapper.GetEnvironmentVariable("EMAIL_SENDER_API_KEY");

//client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
//client.DefaultRequestHeaders.Accept.Add(
//  new MediaTypeWithQualityHeaderValue("application/json"));

////string requestBody = "{"
////                     + "  \"message\": {"
////                     + "    \"recipients\": ["
////                     + "      {"
////                     + "        \"email\": \"user@example.com\","
////                     + "        \"substitutions\": {"
////                     + "          \"CustomerId\": 12452,"
////                     + "          \"to_name\": \"John Smith\""
////                     + "        }"
////                     + "      }"
////                     + "    ],"
////                     + "    \"from_name\": \"John Smith\","
////                     + "    \"reply_to\": \"sena85088@gmail.com\","
////                     + "    \"reply_to_name\": \"John Smith\","
////                     + "  }"
////                     + "}";

//string requestBody = "{"
//    + "  \"message\": {"
//    + "    \"recipients\": ["
//    + "      {"
//    + "        \"email\": \"sena85088@gmail.com\""
//    + "      }"
//    + "    ],"
//    + "    \"body\": {"
//    + "      \"html\": \"<b>Hello, regerge</b>\""
//    + "    },"
//    + "    \"subject\": \"string\""
//    + "  }"
//    + "}";


//var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

//var response = client.PostAsync("email/send.json", content).Result;
//var responseBody = response.Content.ReadAsStringAsync().Result;
//if (response.IsSuccessStatusCode)
//{
//    Console.WriteLine(responseBody);
//}
//else
//{
//    Console.WriteLine(String.Format("Request failed (HTTP {0}): {1}",
//      (int)response.StatusCode, responseBody));
//}


builder.Logging.ClearProviders(); // clear default asp .net core logging

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Host.UseSerilog((hostringContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostringContext.Configuration);
});

var sqliteConnectionStr = builder.Configuration.GetConnectionString("Sqlite")
    ?? throw new NullReferenceException("connection string not found or empty");

builder.Services
    .AddInfastructure(sqliteConnectionStr)
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

app.Use(async (ctx, next) =>
{
    using (LogContext.PushProperty("IPAddress", ctx.Connection.RemoteIpAddress))
    {
        await next(ctx);
    }
});

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

if (app.Environment.IsProduction())
    app.UseProduction();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();