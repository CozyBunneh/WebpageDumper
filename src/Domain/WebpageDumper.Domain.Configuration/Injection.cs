using Microsoft.Extensions.DependencyInjection;

namespace WebpageDumper.Infrastructure.Domain.Configuration;

public static class Injections
{
    public static IServiceCollection AddDomainServices(
      this IServiceCollection services)
    {
        return services;
    }
}