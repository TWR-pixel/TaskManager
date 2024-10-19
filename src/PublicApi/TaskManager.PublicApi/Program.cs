using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

#region get environment variables
var emailApiKey = EnvironmentWrapper.GetEnvironmentVariable("EMAIL_SENDER_API_KEY");
var jwtSecretKey = EnvironmentWrapper.GetEnvironmentVariable("JWT_SECRET_KEY");

if (string.IsNullOrWhiteSpace(emailApiKey))
    throw new NullReferenceException("Environment variable 'EMAIL_API_KEY' not found, or it is empty");

if (string.IsNullOrWhiteSpace(jwtSecretKey))
    throw new NullReferenceException("Environment variable 'JWT_SECRET_KEY' not found, or it is empty");

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<HandleExceptionsMiddleware>();

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

var validIssuer = builder.Configuration["JwtAuthenticationOptions:Issuer"];
var validAudience = builder.Configuration["JwtAuthenticationOptions:Audience"];
var expiresTokenHours = builder.Configuration["JwtAuthenticationOptions:ExpiresTokenHours"];
var expiresTokenMinutes = builder.Configuration["JwtAuthenticationOptions:ExpiresTokenMinutes"];

builder.Services.AddScoped(typeof(IJwtSecurityTokenFactory),
    i => new JwtSecurityTokenFactory(new JwtAuthenticationOptions
    {
        Audience = validAudience ?? "localhost",
        Issuer = validAudience ?? "localhost",
        ExpiresTokenHours = int.Parse(expiresTokenHours!),
        ExpiresTokenMinutes = int.Parse(expiresTokenMinutes!),
        SecretKey = jwtSecretKey
    }));

builder.Services.AddScoped<ISymmetricSecurityKeysGenerator, SymmetricSecurityKeysGenerator>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtRefreshTokenGenerator, JwtRefreshTokenGenerator>();

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