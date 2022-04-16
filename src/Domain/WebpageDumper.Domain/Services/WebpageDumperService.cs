using Microsoft.Extensions.Logging;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Infrastructure.Persistence.Services;
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

    public async Task DumpWebpage(Uri uri)
    {
        var indexPageAsString = await _webService.GetFileAsStringAsync(uri);
        var webpageResources = _webpageParserService.ParseWebpageForResources(uri, indexPageAsString);
    }
}