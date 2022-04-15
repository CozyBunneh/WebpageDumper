using Microsoft.Extensions.Hosting;
using WebpageDumper.Domain.Abstract.Services;

namespace WebpageDumper.Application.WebpageDumper.Console.HostedService;

public class ConsoleService : BackgroundService
{
    private IHostApplicationLifetime _hostApplicationLifetime;
    private IWebpageDumperService _webpageDumperService;

    public ConsoleService(IHostApplicationLifetime hostApplicationLifetime, IWebpageDumperService webpageDumperService)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _webpageDumperService = webpageDumperService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var urlString = Environment.GetCommandLineArgs()[1];
        var uri = new UriBuilder(urlString);
        var pageName = uri.Host;

        await _webpageDumperService.DumpWebpage(uri.Uri);

        _hostApplicationLifetime.StopApplication();
    }
}