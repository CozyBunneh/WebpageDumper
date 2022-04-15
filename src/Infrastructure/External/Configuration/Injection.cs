using Microsoft.Extensions.DependencyInjection;

namespace WebpageDumper.Infrastructure.External.Configuration;

public static class Injections
{
    public static IServiceCollection AddExternalServices(
      this IServiceCollection services)
    {
        return services;
    }
}
