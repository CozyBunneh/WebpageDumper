using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Infrastructure.Webpage.Abstract.Services;

public interface IWebpageFileParserService
{
    IList<WebpageResource> ParserFileForInternalLinks(Uri pageUri, String fileAsString);
}