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
using TaskManager.Persistence.Sqlite.Role;
using TaskManager.Persistence.Sqlite.User;
using TaskManager.Persistence.Sqlite.UserTask;
using TaskManager.Persistence.Sqlite.UserTaskColumn;

namespace TaskManager.Persistence.Sqlite.Common.Extensions;

public static class PersistenceSqliteServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services
            .AddDbContext(connectionString)
            .AddRepositories()
            .AddReadRepositories()
            .AddUnitOfWorks();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TaskManagerDbContext>(d => d.UseSqlite(connectionString));

        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentityCore<UserEntity>()
            .AddRoles<RoleEntity>()
            .AddEntityFrameworkStores<TaskManagerDbContext>();


        return services;
    }

    public static IServiceCollection AddReadRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IReadonlyRepository<UserEntity>, UserRepository>()
            .AddScoped<IReadonlyRepository<RoleEntity>, RoleRepository>()
            .AddScoped<IReadonlyRepository<UserTaskEntity>, UserTaskRepository>()
            .AddScoped<IReadonlyRepository<UserTaskColumnEntity>, UserTaskColumnRepository>();

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
            .AddScoped<IReadonlyUnitOfWork, ReadonlyUnitOfWork>();

        return services;
    }


}
