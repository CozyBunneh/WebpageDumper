using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Infrastructure.Webpage.Abstract.Service;

public interface IWebService
{
    Task<Stream> GetFileStreamAsync(Uri uri);
    Task<String> GetFileAsStringAsync(Uri uri);
    Task<Stream> GetWebpageResourceAsStreamAsync(Uri uri, WebpageResource webpageResource);
    Task<String> GetWebpageResourceAsStringAsync(Uri uri, WebpageResource webpageResource);
}