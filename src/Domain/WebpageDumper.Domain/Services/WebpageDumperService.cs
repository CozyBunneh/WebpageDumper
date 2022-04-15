using Microsoft.Extensions.Logging;
using WebpageDumper.Domain.Abstract.Services;

namespace WebpageDumper.Domain.Services;

public class WebpageDumperService : IWebpageDumperService
{
    ILogger<WebpageDumperService> _logger;

    public WebpageDumperService(ILogger<WebpageDumperService> logger)
    {
        _logger = logger;
    }

    public Task DumpWebpage(Uri uri)
    {
        return Task.CompletedTask;
    }
}