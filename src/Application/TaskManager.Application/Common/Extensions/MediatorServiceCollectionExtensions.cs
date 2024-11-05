using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManager.Application.Common.Requests.Handlers;

namespace TaskManager.Application.Common.Extensions;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(RequestExceptionHandler<,,>));

        return services;
    }
}
