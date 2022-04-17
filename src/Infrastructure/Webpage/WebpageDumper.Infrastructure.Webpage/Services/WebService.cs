using Microsoft.Extensions.Logging;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;
using WebpageDumper.Infrastructure.Webpage.Abstract.Service;

namespace WebpageDumper.Infrastructure.Webpage.Service;

public class WebService : IWebService
{
    private ILogger<WebService> _logger;
    private HttpClient _httpClient;

    public WebService(ILogger<WebService> logger)
    {
        _logger = logger;

        // One HttpClient per service instance to allow fo threading
        _httpClient = new HttpClient();
    }

    public Task<Stream> GetFileStreamAsync(Uri uri)
    {
        return _httpClient.GetStreamAsync(uri);
    }

    public Task<String> GetFileAsStringAsync(Uri uri)
    {
        return _httpClient.GetStringAsync(uri);
    }

    public Task<String> GetWebpageResourceAsStringAsync(Uri uri, WebpageResource webpageResource)
    {
        return GetFileAsStringAsync(GetWebpageResourceUri(uri, webpageResource));
    }

    private Uri GetWebpageResourceUri(Uri uri, WebpageResource webpageResource)
    {
        var uriBuilder = new UriBuilder(uri);
        uriBuilder.Path = $"/{webpageResource.path}/{webpageResource.fileName}";
        return uriBuilder.Uri;
    }
}