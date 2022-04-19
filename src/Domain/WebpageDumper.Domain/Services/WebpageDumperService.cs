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

    /// <summary>
    ///     Main function for dumping webpages, first it fetches the index.html as a string,
    ///     parses it for files on the server and later downloads them all in a threaded manner.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task DumpWebpage(DownloadWebpageCommand command)
    {
        var (indexPageAsStringTask, updatedCommand) = GetWebpageIndexAsString(command);
        var indexPageAsString = await indexPageAsStringTask;
        command = updatedCommand;

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

    /// <summary>
    ///     Method that fetches a webpage as string so that it can be parsed.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lastTry">Only do one recursive retry, no infinite looping</param>
    /// <returns></returns>
    private (Task<String>, DownloadWebpageCommand) GetWebpageIndexAsString(DownloadWebpageCommand command, bool lastTry = false)
    {
        try
        {
            return (_spinnerLoaderService.GetWebpageIndex(command.uri), command);
        }
        catch (AggregateException) // Received on 404 errors, for google.com a retry with www.google.com is performed or vice versa.ß
        {
            command = ChangeUriToBeWithOrWithoutWww(command);
            if (!lastTry)
            {
                return GetWebpageIndexAsString(command, true);
            }
            else
            {
                return (CouldNotGetIndexAsString(), command);
            }
        }
    }

    /// <summary>
    ///     Write error message to console.
    /// </summary>
    /// <returns></returns>
    private Task<String> CouldNotGetIndexAsString()
    {
        Console.WriteLine(UnableToGetFromAddress);
        return (Task<String>)Task.CompletedTask;
    }

    /// <summary>
    ///     Simple method that fo instance adds www to google.com resulting in www.google.com,
    ///     or if www was present removes it.ß
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
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