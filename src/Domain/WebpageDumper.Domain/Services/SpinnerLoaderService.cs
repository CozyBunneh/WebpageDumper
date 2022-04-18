using Kurukuru;
using Microsoft.Extensions.Logging;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;
using WebpageDumper.Infrastructure.Webpage.Abstract.Service;
using WebpageDumper.Infrastructure.Webpage.Abstract.Services;

namespace WebpageDumper.Domain.Services;

public class SpinnerLoaderService : ISpinnerLoaderService
{
    private const String FetchingIndex = "Fetching index from webpage.";
    private const String ParsingIndex = "Parsing index page for resource links.";
    private const String IndexPageFound = "Index page found for: ";
    private const String IndexPageFailure = "Unable to get index page from: ";
    private const String ResourcesFound = " resources found.";
    private const String NoResourcesFound = "No resources where found.";
    private const ConsoleColor Color = ConsoleColor.Cyan;
    private static readonly Pattern PrimaryPattern = Patterns.Dots12;
    private static readonly Pattern FallbackPattern = Patterns.Flip;

    private ILogger<ISpinnerLoaderService> _logger;
    private IWebService _webService;
    private IWebpageParserService _webpageParserService;

    public SpinnerLoaderService(ILogger<ISpinnerLoaderService> logger, IWebService webService, IWebpageParserService webpageParserService)
    {
        _logger = logger;
        _webService = webService;
        _webpageParserService = webpageParserService;
    }

    public async Task<String> GetWebpageIndex(Uri uri)
    {
        var indexPageAsString = "";
        await Spinner.StartAsync(FetchingIndex, async spinner =>
        {
            spinner.Color = Color;
            indexPageAsString = await _webService.GetFileAsStringAsync(uri);
            spinner.Text = GetWebpageIndexResultText(uri, indexPageAsString);
        }, PrimaryPattern, FallbackPattern);
        return indexPageAsString;
    }

    public IList<WebpageResource> GetWebresourcesLinksFromIndexPage(Uri uri, String indexPageAsString)
    {
        IList<WebpageResource> webpageResources = new List<WebpageResource>();
        Spinner.Start(ParsingIndex, spinner =>
        {
            spinner.Color = Color;
            webpageResources = _webpageParserService.ParseWebpageForResources(uri, indexPageAsString);
            spinner.Text = GetWebpageIndexAndWebresourceLinksResultText(uri, indexPageAsString, webpageResources);
        }, PrimaryPattern, FallbackPattern);
        return webpageResources;
    }

    private String GetWebpageIndexResultText(Uri uri, String indexPageAsString)
    {
        return indexPageAsString != "" ?
            $"{IndexPageFound}{uri.ToString()}." :
            $"{IndexPageFailure}{uri.ToString()}.";
    }

    private String GetWebpageIndexAndWebresourceLinksResultText(Uri uri, String indexPageAsString, IList<WebpageResource> webpageResources)
    {
        return webpageResources.Count > 0 ?
            $"{webpageResources.Count}{ResourcesFound}" :
            NoResourcesFound;
    }
}