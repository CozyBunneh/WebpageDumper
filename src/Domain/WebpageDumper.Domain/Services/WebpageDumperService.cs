using Microsoft.Extensions.Logging;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Infrastructure.Persistence.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Service;

namespace WebpageDumper.Domain.Services;

public class WebpageDumperService : IWebpageDumperService
{
    private ILogger<WebpageDumperService> _logger;
    private IWebService _webService;
    private IFileService _fileService;

    public WebpageDumperService(ILogger<WebpageDumperService> logger,
                                IWebService webService,
                                IFileService fileService)
    {
        _logger = logger;
        _webService = webService;
        _fileService = fileService;
    }

    public async Task DumpWebpage(Uri uri)
    {
        String pageName = uri.Host;

        Stream indexStream = await _webService.GetFileStreamAsync(uri);
        await _fileService.WriteFileToPath(indexStream, "index.html", pageName);
    }
}