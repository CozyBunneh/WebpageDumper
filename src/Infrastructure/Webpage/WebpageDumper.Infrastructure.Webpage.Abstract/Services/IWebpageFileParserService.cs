namespace WebpageDumper.Infrastructure.Webpage.Abstract.Services;

public interface IWebpageFileParserService
{
    IEnumerable<String> ParserFileForInternalLinks(String fileAsString);
}