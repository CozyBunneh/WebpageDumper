using Microsoft.Extensions.Logging;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Domain.Services;

public class WebpageDumperService : IWebpageDumperService
{
    private const int DefaultNumberOfThreads = 4;

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

    public async Task DumpWebpage(Uri uri, int numberOfThreads = DefaultNumberOfThreads)
    {
        var indexPageAsString = await _spinnerLoaderService.GetWebpageIndex(uri);
        if (indexPageAsString == null || indexPageAsString == "")
        {
            return;
        }

        IList<WebpageResource> webpageResources = _spinnerLoaderService.GetWebresourcesLinksFromIndexPage(
            uri,
            indexPageAsString);
        if (webpageResources.Count == 0)
        {
            return;
        }

        await _progressLoaderService.DownloadWebpageResources(uri, numberOfThreads, webpageResources);
    }
}