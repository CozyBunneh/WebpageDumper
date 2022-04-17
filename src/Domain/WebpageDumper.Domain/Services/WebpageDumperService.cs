using Kurukuru;
using Microsoft.Extensions.Logging;
using ShellProgressBar;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Infrastructure.Persistence.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;
using WebpageDumper.Infrastructure.Webpage.Abstract.Service;
using WebpageDumper.Infrastructure.Webpage.Abstract.Services;

namespace WebpageDumper.Domain.Services;

public class WebpageDumperService : IWebpageDumperService
{
    private ILogger<WebpageDumperService> _logger;
    private IWebService _webService;
    private IWebpageParserService _webpageParserService;
    private IFileService _fileService;


    public WebpageDumperService(ILogger<WebpageDumperService> logger,
                                IWebService webService,
                                IWebpageParserService webpageParserService,
                                IFileService fileService)
    {
        _logger = logger;
        _webService = webService;
        _webpageParserService = webpageParserService;
        _fileService = fileService;
    }

    public async Task DumpWebpage(Uri uri, int numberOfThreads = 4)
    {
        var indexPageAsString = await GetWebpageIndex(uri);
        if (indexPageAsString == null || indexPageAsString == "")
        {
            return;
        }

        IList<WebpageResource> webpageResources = GetWebresourcesLinksFromIndexPage(uri, indexPageAsString);
        if (webpageResources.Count == 0)
        {
            return;
        }

        Console.WriteLine($"Starting download of resources using {numberOfThreads} threads...");

        var options = new ProgressBarOptions
        {
            ForegroundColor = ConsoleColor.Yellow,
            BackgroundColor = ConsoleColor.DarkYellow,
            ProgressCharacter = '─'
        };

        using (var progressBar = new ProgressBar(webpageResources.Count, $"Downloading resources from: {uri.ToString()}", options))
        {
            foreach (var webpageResource in webpageResources)
            {
                await _webService.GetWebpageResourceAsStringAsync(uri, webpageResource);
                progressBar.Tick();
            }
        }
    }

    private async Task<String> GetWebpageIndex(Uri uri)
    {
        var indexPageAsString = "";
        await Spinner.StartAsync($"Fetching index from webpage.", async spinner =>
        {
            spinner.Color = ConsoleColor.Cyan;
            indexPageAsString = await _webService.GetFileAsStringAsync(uri);
            spinner.Text = GetWebpageIndexResultText(uri, indexPageAsString);
        }, Patterns.Dots12, Patterns.Flip);
        return indexPageAsString;
    }

    private IList<WebpageResource> GetWebresourcesLinksFromIndexPage(Uri uri, String indexPageAsString)
    {
        IList<WebpageResource> webpageResources = new List<WebpageResource>();
        Spinner.Start($"Parsing index page for resource links.", spinner =>
        {
            spinner.Color = ConsoleColor.Cyan;
            webpageResources = _webpageParserService.ParseWebpageForResources(uri, indexPageAsString);
            spinner.Text = GetWebpageIndexAndWebresourceLinksResultText(uri, indexPageAsString, webpageResources);
        }, Patterns.Dots12, Patterns.Flip);
        return webpageResources;
    }

    private String GetWebpageIndexResultText(Uri uri, String indexPageAsString)
    {
        return indexPageAsString != "" ?
            $"Index page found for: {uri.ToString()}" :
            $"Unable to get index page from: {uri.ToString()}.";
    }

    private String GetWebpageIndexAndWebresourceLinksResultText(Uri uri, String indexPageAsString, IList<WebpageResource> webpageResources)
    {
        return webpageResources.Count > 0 ?
            $"{webpageResources.Count} resources found." :
            $"No resources where found.";
    }
}