using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.TaskColumns;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Domain.UseCases.Users;
using TaskManager.Infrastructure.Sqlite.Role;
using TaskManager.Infrastructure.Sqlite.User;
using TaskManager.Infrastructure.Sqlite.UserTask;
using TaskManager.Infrastructure.Sqlite.UserTaskColumn;

namespace TaskManager.Infrastructure.Sqlite.Common.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services
            .AddTaskManagerDbContext(connectionString)
            .AddRepositories()
            .AddReadRepositories()
            .AddUnitOfWorks();

        return services;
    }

    public static IServiceCollection AddTaskManagerDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TaskManagerDbContext>(d => d.UseSqlite(connectionString));

        return services;
    }

    public static IServiceCollection AddReadRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IReadableRepository<UserEntity>, UserRepository>()
            .AddScoped<IReadableRepository<RoleEntity>, RoleRepository>()
            .AddScoped<IReadableRepository<UserTaskEntity>, UserTaskRepository>()
            .AddScoped<IReadableRepository<UserTaskColumnEntity>, UserTaskColumnRepository>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IUserTaskRepository, UserTaskRepository>()
            .AddScoped<IUserTaskColumnRepository, UserTaskColumnRepository>();

        return services;
    }

    public static IServiceCollection AddUnitOfWorks(this IServiceCollection services)
    {
        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IReadUnitOfWork, ReadUnitOfWork>();

        return services;
    }
}
