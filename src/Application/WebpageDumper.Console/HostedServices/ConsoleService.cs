using Microsoft.Extensions.Hosting;
using WebpageDumper.Infrastructure.External.Abstract.Service;

namespace WebpageDumper.Application.WebpageDumper.Console.HostedService;

public class ConsoleService : BackgroundService
{
    private IHostApplicationLifetime _hostApplicationLifetime;
    private IWebService _webService;

    public ConsoleService(IHostApplicationLifetime hostApplicationLifetime, IWebService webService)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _webService = webService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // await _webService.GetFileListAsync();

        _hostApplicationLifetime.StopApplication();
    }
}