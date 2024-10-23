using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.UseCases.Common.Repositories;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Infastructure.Sqlite.Common.Extensions;

public static class InfastructureServiceCollectionExtensions
{
    public static IServiceCollection AppInfastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TaskManagerDbContext>(d => d.UseSqlite(connectionString));

        services
            .AddScoped<IRepositoryBase<UserEntity>, EfRepository<UserEntity>>()
            .AddScoped<IRepositoryBase<RoleEntity>, EfRepository<RoleEntity>>()
            .AddScoped<IRepositoryBase<UserTaskEntity>, EfRepository<UserTaskEntity>>()
            .AddScoped<IRepositoryBase<TaskColumnEntity>, EfRepository<TaskColumnEntity>>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}
