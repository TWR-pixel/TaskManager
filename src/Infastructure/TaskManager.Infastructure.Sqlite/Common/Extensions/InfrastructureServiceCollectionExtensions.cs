using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

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
            .AddScoped<IReadRepositoryBase<UserEntity>, EfRepository<UserEntity>>()
            .AddScoped<IReadRepositoryBase<RoleEntity>, EfRepository<RoleEntity>>()
            .AddScoped<IReadRepositoryBase<UserTaskEntity>, EfRepository<UserTaskEntity>>()
            .AddScoped<IReadRepositoryBase<UserTaskColumnEntity>, EfRepository<UserTaskColumnEntity>>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IRepositoryBase<UserEntity>, EfRepository<UserEntity>>()
            .AddScoped<IRepositoryBase<RoleEntity>, EfRepository<RoleEntity>>()
            .AddScoped<IRepositoryBase<UserTaskEntity>, EfRepository<UserTaskEntity>>()
            .AddScoped<IRepositoryBase<UserTaskColumnEntity>, EfRepository<UserTaskColumnEntity>>();

        return services;
    }

    public static IServiceCollection AddUnitOfWorks(this IServiceCollection services)
    {
        services
            .AddScoped<IUnitOfWork, EfUnitOfWork>()
            .AddScoped<IReadUnitOfWork, EfReadUnitOfWork>();

        return services;
    }
}
