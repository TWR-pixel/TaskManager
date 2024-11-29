using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.File;

namespace TaskManager.Infrastructure.File;

public static class InfrastructureFileServiceCollectionExtensions
{
    public static IServiceCollection AddFileServices(this IServiceCollection services, IConfiguration configuration)
    {
        var pathForUserProfileImages = Directory.GetCurrentDirectory() + configuration["PathForUserProfileImages"];

        if (!Directory.Exists(pathForUserProfileImages))
            Directory.CreateDirectory(pathForUserProfileImages);

        services.Configure<FileWriterOptions>(options =>
        {
            options.PathForUserProfileImages = pathForUserProfileImages;
        });

        services.AddScoped<IRandomFileNameGenerator, RandomFileNameGenerator>();
        services.AddScoped<IFileWriter, FileWriter>();
        services.AddScoped<IFileReader, FileReader>();

        return services;
    }
}
