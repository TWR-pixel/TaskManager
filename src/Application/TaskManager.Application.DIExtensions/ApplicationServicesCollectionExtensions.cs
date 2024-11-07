using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.User.Common.Email.Common.Extensions;

namespace TaskManager.Application.DIExtensions;

public static class ApplicationServicesCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddEmailVerification()
            .AddSecurity()
            .AddMediatR();

        return services;
    }
}
