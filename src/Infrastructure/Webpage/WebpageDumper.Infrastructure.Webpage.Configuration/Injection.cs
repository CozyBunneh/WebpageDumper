using Microsoft.Extensions.DependencyInjection;
using WebpageDumper.Infrastructure.Webpage.Abstract.Service;
using WebpageDumper.Infrastructure.Webpage.Abstract.Services;
using WebpageDumper.Infrastructure.Webpage.Service;
using WebpageDumper.Infrastructure.Webpage.Services;

namespace WebpageDumper.Infrastructure.Webpage.Configuration;

public static class Injections
{
    public static IServiceCollection AddWebpageServices(
      this IServiceCollection services)
    {
        services.AddTransient<IWebService, WebService>();
        services.AddTransient<IWebpageFileParserService, WebpageFileParserService>();
        return services;
    }
}
