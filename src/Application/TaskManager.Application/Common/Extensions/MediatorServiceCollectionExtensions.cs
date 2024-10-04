using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskManager.Application.Common.Extensions;

public static class MediatorServiceCollectionExtensions
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
