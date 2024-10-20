using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Security.Auth.Jwt.Claims;
using TaskManager.Application.Common.Security.Auth.Jwt.Options;
using TaskManager.Application.Common.Security.Auth.Jwt.Tokens;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Application.Common.Security.Hashers.BCrypt;
using TaskManager.Application.Common.Security.SymmetricSecurityKeys;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.UseCases.Common.Repositories;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Infastructure.Sqlite;
using TaskManager.Infastructure.Sqlite.Common;
using TaskManager.PublicApi.Common.Middlewares;
using TaskManager.PublicApi.Common.Wrappers;
using TaskManager.PublicApi.Common.Wrappers.Mediator;
using TaskManager.Application.Common.Services.EmailSender;
using TaskManager.Application.Common.Services.EmailVerifier;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<HandleExceptionsMiddleware>();

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

var sqliteConnectionStr = builder.Configuration.GetConnectionString("Sqlite");

builder.Services.AddDbContext<TaskManagerDbContext>(d => d.UseSqlite(sqliteConnectionStr));

#region Add entityframework repositories
builder.Services.AddScoped<IRepositoryBase<UserEntity>, EfRepository<UserEntity>>();
builder.Services.AddScoped<IRepositoryBase<RoleEntity>, EfRepository<RoleEntity>>();
builder.Services.AddScoped<IRepositoryBase<UserTaskEntity>, EfRepository<UserTaskEntity>>();
builder.Services.AddScoped<IRepositoryBase<TaskColumnEntity>, EfRepository<TaskColumnEntity>>();
#endregion

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddMediator();
builder.Services.AddScoped<IMediatorWrapper, MediatorWrapper>();

#region authentication services

builder.Services.AddScoped<IJwtClaimsFactory, JwtClaimsFactory>();
builder.Services.AddScoped<IJwtSecurityTokenFactory, JwtSecurityTokenFactory>();

var emailApiKey = EnvironmentWrapper.GetEnvironmentVariable("EMAIL_SENDER_API_KEY");
var jwtSecretKey = EnvironmentWrapper.GetEnvironmentVariable("JWT_SECRET_KEY");

var validIssuer = builder.Configuration["JwtAuthenticationOptions:Issuer"];
var validAudience = builder.Configuration["JwtAuthenticationOptions:Audience"];
var expiresTokenHours = builder.Configuration["JwtAuthenticationOptions:ExpiresTokenHours"];
var expiresTokenMinutes = builder.Configuration["JwtAuthenticationOptions:ExpiresTokenMinutes"];

var emailFrom = builder.Configuration["EmailSenderOptions:EmailFrom"]!;
var smtpHost = builder.Configuration["EmailSenderOptions:Host"]!;

var smtpPort = builder.Configuration.GetValue<int>("EmailSenderOptions:Port");

builder.Services.Configure<EmailSenderOptions>(options =>
{
    options.From = emailFrom;
    options.Host = smtpHost;
    options.Port = smtpPort;
    options.Password = emailApiKey;
    options.SmtpClient = new SmtpClient(smtpHost, smtpPort)
    {
        EnableSsl = true,
        Credentials = new NetworkCredential(emailFrom, emailApiKey),
    };
});

builder.Services.Configure<JwtAuthenticationOptions>(options =>
{
    options.Audience = validAudience ?? "localhost";
    options.Issuer = validAudience ?? "localhost";
    options.ExpiresTokenHours = int.Parse(expiresTokenHours!);
    options.ExpiresTokenMinutes = int.Parse(expiresTokenMinutes!);
    options.SecretKey = jwtSecretKey;
});

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEmailVerifier, EmailVerifier>();
builder.Services.AddScoped<ISymmetricSecurityKeysGenerator, SymmetricSecurityKeysGenerator>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

#region configure jwt bearer authentication scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

#endregion

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