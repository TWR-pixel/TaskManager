using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManager.Application.Common.Extensions;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;
using TaskManager.Application.Common.Security.Authentication.JwtAuth.Options;
using TaskManager.Application.Common.Security.Authentication.JwtClaims;
using TaskManager.Application.Common.Security.Hashers;
using TaskManager.Application.Common.Security.Hashers.BCrypt;
using TaskManager.Application.Common.Security.SymmetricSecurityKeys;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;
using TaskManager.PublicApi.Common;
using TaskManager.PublicApi.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<HandleExceptionsMiddleware>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

var connectionString = builder.Configuration.GetConnectionString("Sqlite");

builder.Services.AddDbContext<TaskManagerDbContext>(d =>
{
    d.UseSqlite(connectionString);
});

#region Add entityframework repositories
builder.Services.AddScoped<EfRepositoryBase<UserEntity>, EfRepository<UserEntity>>();
builder.Services.AddScoped<EfRepositoryBase<RoleEntity>, EfRepository<RoleEntity>>();
builder.Services.AddScoped<EfRepositoryBase<UserTaskEntity>, EfRepository<UserTaskEntity>>();
builder.Services.AddScoped<EfRepositoryBase<TaskColumnEntity>, EfRepository<TaskColumnEntity>>();
#endregion

builder.Services.AddMediator();
builder.Services.AddScoped<IMediatorFacade, MediatorFacade>();

#region Add authentication services

builder.Services.AddScoped<IJwtClaimsFactory, JwtClaimsFactory>();
builder.Services.AddScoped<IJwtSecurityTokenFactory, JwtSecurityTokenFactory>();
builder.Services.AddScoped<ISymmetricSecurityKeysGenerator, SymmetricSecurityKeysGenerator>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtRefreshTokenGenerator, JwtRefreshTokenGenerator>();

builder.Services.AddOptions<JwtAuthenticationOptions>().BindConfiguration(nameof(JwtAuthenticationOptions),
    o => o.ErrorOnUnknownConfiguration = true);

var validIssuer = builder.Configuration["JwtAuthenticationOptions:Issuer"];
var validAudience = builder.Configuration["JwtAuthenticationOptions:Audience"];
var issuerSigningKey = builder.Configuration["JwtAuthenticationOptions:SecurityKey"];

if (string.IsNullOrWhiteSpace(issuerSigningKey))
    throw new NullReferenceException(nameof(issuerSigningKey) + " is null or empty");

builder.Services.AddAuthentication();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey)),
            ValidateIssuerSigningKey = true,
        };
    });


builder.Services.AddMemoryCache();
#endregion

#endregion

var app = builder.Build();

app.UseMiddleware<HandleExceptionsMiddleware>(); // catches all exceptions in app and logging them

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
