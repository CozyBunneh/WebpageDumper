using Microsoft.Extensions.DependencyInjection;
using WebpageDumper.Infrastructure.Persistence.Services;

namespace WebpageDumper.Infrastructure.Persistence.Configuration;

public static class Injections
{
    public static IServiceCollection AddPersistenceServices(
      this IServiceCollection services)
    {
        services.AddTransient<IStorageService, FileService>();

        return services;
    }
}