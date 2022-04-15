using Microsoft.Extensions.DependencyInjection;
using WebpageDumper.Infrastructure.External.Abstract.Service;
using WebpageDumper.Infrastructure.External.Service;

namespace WebpageDumper.Infrastructure.External.Configuration;

public static class Injections
{
    public static IServiceCollection AddExternalServices(
      this IServiceCollection services)
    {
        services.AddTransient<IWebService, WebService>();
        return services;
    }
}
