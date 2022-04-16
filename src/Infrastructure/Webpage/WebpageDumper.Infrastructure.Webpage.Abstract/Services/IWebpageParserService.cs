using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Infrastructure.Webpage.Abstract.Services;

public interface IWebpageParserService
{
    IList<WebpageResource> ParseWebpageForResources(Uri pageUri, String fileAsString);
}