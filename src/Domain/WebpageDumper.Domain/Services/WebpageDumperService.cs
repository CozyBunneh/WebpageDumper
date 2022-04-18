using Microsoft.Extensions.Logging;
using WebpageDumper.Domain.Abstract.Commands;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Domain.Services;

public class WebpageDumperService : IWebpageDumperService
{
    private const String Www = "www";
    private const String UnableToGetFromAddress = "Unable to get index.html from the provided address.";

    private ILogger<WebpageDumperService> _logger;
    private ISpinnerLoaderService _spinnerLoaderService;
    private IProgressLoaderService _progressLoaderService;

    public WebpageDumperService(
        ILogger<WebpageDumperService> logger,
        ISpinnerLoaderService spinnerLoaderService,
        IProgressLoaderService progressLoaderService)
    {
        _logger = logger;
        _spinnerLoaderService = spinnerLoaderService;
        _progressLoaderService = progressLoaderService;
    }

    public async Task DumpWebpage(DownloadWebpageCommand command)
    {
        var indexPageAsString = await GetWebpageIndexAsString(command);
        if (indexPageAsString == null || indexPageAsString == "")
        {
            return;
        }

        IList<WebpageResource> webpageResources = _spinnerLoaderService.GetWebresourcesLinksFromIndexPage(
            command.uri,
            indexPageAsString);
        if (webpageResources.Count == 0)
        {
            return;
        }

        _progressLoaderService.DownloadWebpageResources(
            command,
            webpageResources);
    }

    private Task<String> GetWebpageIndexAsString(DownloadWebpageCommand command, bool lastTry = false)
    {
        try
        {
            return _spinnerLoaderService.GetWebpageIndex(command.uri);
        }
        catch (AggregateException)
        {
            command = ChangeUriToBeWithOrWithoutWww(command);
            if (!lastTry)
            {
                return GetWebpageIndexAsString(command, true);
            }
            else
            {
                return CouldNotGetIndexAsString();
            }
        }
    }

    private Task<String> CouldNotGetIndexAsString()
    {
        Console.WriteLine(UnableToGetFromAddress);
        return (Task<String>)Task.CompletedTask;
    }

    private DownloadWebpageCommand ChangeUriToBeWithOrWithoutWww(DownloadWebpageCommand command)
    {
        Uri newUri;
        if (command.uri.Host.Contains(Www))
        {
            var hostAddress = command.uri.Host.Replace($"{Www}.", "");
            newUri = new UriBuilder($"{command.uri.Scheme}://{hostAddress}").Uri;
        }
        else
        {
            newUri = new UriBuilder($"{command.uri.Scheme}://{Www}.{command.uri.Host}").Uri;
        }
        return new DownloadWebpageCommand(newUri, command.numberOfThreads, command.output);
    }
}