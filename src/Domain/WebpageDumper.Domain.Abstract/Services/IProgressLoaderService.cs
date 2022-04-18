using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Domain.Abstract.Services;

public interface IProgressLoaderService
{
    Task DownloadWebpageResources(Uri uri, int numberOfThreads, IList<WebpageResource> webpageResources);
}