using Microsoft.Extensions.Logging;
using WebpageDumper.Infrastructure.External.Abstract.Service;

namespace WebpageDumper.Infrastructure.External.Service;

public class WebService : IWebService
{
    ILogger<WebService> _logger;

    public WebService(ILogger<WebService> logger)
    {
        _logger = logger;
    }

    public Task<Stream> GetFileStreamAsync(Uri uri)
    {
        var httpClient = new HttpClient();
        return httpClient.GetStreamAsync(uri);
    }
}