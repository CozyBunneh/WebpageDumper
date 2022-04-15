using Microsoft.Extensions.DependencyInjection;

namespace WebpageDumper.Infrastructure.Persistence.Configuration;

public static class Injections
{
    public static IServiceCollection AddPersistenceServices(
      this IServiceCollection services)
    {
        return services;
    }
}