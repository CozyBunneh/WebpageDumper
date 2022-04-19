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

    public Task<Stream> GetWebpageResourceAsStreamAsync(Uri uri, WebpageResource webpageResource)
    {
        return _httpClient.GetStreamAsync(GetWebpageResourceUri(uri, webpageResource));
    }

    public Task<String> GetWebpageResourceAsStringAsync(Uri uri, WebpageResource webpageResource)
    {
        return _httpClient.GetStringAsync(GetWebpageResourceUri(uri, webpageResource));
    }

    private Uri GetWebpageResourceUri(Uri uri, WebpageResource webpageResource)
    {
        var uriBuilder = new UriBuilder(uri);
        uriBuilder.Path = $"/{webpageResource.path}/{webpageResource.fileName}";
        return uriBuilder.Uri;
    }
}