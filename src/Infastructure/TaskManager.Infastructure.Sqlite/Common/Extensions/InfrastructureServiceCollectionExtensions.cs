﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.DALImplementation.Sqlite.Role;
using TaskManager.DALImplementation.Sqlite.User;
using TaskManager.DALImplementation.Sqlite.UserBoard;
using TaskManager.DALImplementation.Sqlite.UserTask;
using TaskManager.DALImplementation.Sqlite.UserTaskColumn;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.UserBoard;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.TaskColumns;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Domain.UseCases.UserBoard;
using TaskManager.Domain.UseCases.Users;

namespace TaskManager.DALImplementation.Sqlite.Common.Extensions;

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
            .AddScoped<IReadRepositoryBase<UserEntity>, UserRepository>()
            .AddScoped<IReadRepositoryBase<RoleEntity>, RoleRepository>()
            .AddScoped<IReadRepositoryBase<UserTaskEntity>, UserTaskRepository>()
            .AddScoped<IReadRepositoryBase<UserTaskColumnEntity>, UserTaskColumnRepository>()
            .AddScoped<IReadRepositoryBase<UserBoardEntity>, UserBoardRepository>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IUserTaskRepository, UserTaskRepository>()
            .AddScoped<IUserTaskColumnRepository, UserTaskColumnRepository>()
            .AddScoped<IUserBoardRepository, UserBoardRepository>();

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
