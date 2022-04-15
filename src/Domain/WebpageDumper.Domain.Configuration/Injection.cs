using Microsoft.Extensions.DependencyInjection;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Domain.Services;

namespace WebpageDumper.Infrastructure.Domain.Configuration;

public static class Injections
{
    public static IServiceCollection AddDomainServices(
      this IServiceCollection services)
    {
        services.AddTransient<IWebpageDumperService, WebpageDumperService>();

        return services;
    }
}