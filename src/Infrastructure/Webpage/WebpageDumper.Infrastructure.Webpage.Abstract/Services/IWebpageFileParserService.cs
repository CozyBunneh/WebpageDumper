namespace WebpageDumper.Infrastructure.Webpage.Abstract.Services;

public interface IWebpageFileParserService
{
    IList<String> ParserFileForInternalLinks(Uri pageUri, String fileAsString);
}