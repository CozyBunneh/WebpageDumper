using CommandLine;
using Microsoft.Extensions.Hosting;
using WebpageDumper.Console.Models;
using WebpageDumper.Domain.Abstract.Commands;
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
        await ParseCommandLineArguments();
        _hostApplicationLifetime.StopApplication();
    }

    private async Task ParseCommandLineArguments()
    {
        await Parser.Default.ParseArguments<CommandLineOptions>(Environment.GetCommandLineArgs())
                            .WithParsedAsync(RunAsync);
    }

    private async Task RunAsync(CommandLineOptions options)
    {
        if (options.WebpageAddress != null && options.Output != null)
        {
            var uri = new UriBuilder(options.WebpageAddress);
            await _webpageDumperService.DumpWebpage(new DownloadWebpageCommand(
                uri.Uri,
                options.Threads,
                options.Output));
        }
    }
}