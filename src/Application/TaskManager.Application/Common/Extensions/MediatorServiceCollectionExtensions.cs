using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Requests.Handlers;

namespace TaskManager.Application.Common.Extensions;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRRequestHandlers(this IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddRequestExceptionHandler();

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    public static IServiceCollection AddRequestExceptionHandler(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(RequestExceptionHandler<,,>));

        return services;
    }
}
