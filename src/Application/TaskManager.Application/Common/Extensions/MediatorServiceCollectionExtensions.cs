using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Application.Role;
using TaskManager.Application.User;
using TaskManager.Application.UserBoard;
using TaskManager.Application.UserTask;
using TaskManager.Application.UserTaskColumn;

namespace TaskManager.Application.Common.Extensions;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRRequestHandlers(this IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(typeof(UserDto).Assembly);
            c.RegisterServicesFromAssembly(typeof(RoleDto).Assembly);
            c.RegisterServicesFromAssembly(typeof(UserTaskDto).Assembly);
            c.RegisterServicesFromAssembly(typeof(UserTaskColumnDto).Assembly);
            c.RegisterServicesFromAssembly(typeof(UserBoardDto).Assembly);
        });

        services.AddRequestExceptionHandler();

        services.AddValidatorsFromAssembly(typeof(UserDto).Assembly);

        return services;
    }

    public static IServiceCollection AddRequestExceptionHandler(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(RequestExceptionHandler<,,>));

        return services;
    }
}
