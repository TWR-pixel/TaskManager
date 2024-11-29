using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Infrastructure.Validator;

public static class InfrastructureValidatorsServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
