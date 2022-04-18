using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Domain.Abstract.Services;

public interface ISpinnerLoaderService
{
    Task<String> GetWebpageIndex(Uri uri);
    IList<WebpageResource> GetWebresourcesLinksFromIndexPage(Uri uri, String indexPageAsString);
}