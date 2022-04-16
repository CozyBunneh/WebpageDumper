using System.Text.RegularExpressions;
using WebpageDumper.Infrastructure.Webpage.Abstract.Services;

namespace WebpageDumper.Infrastructure.Webpage.Services;

public class WebpageFileParserService : IWebpageFileParserService
{
    private Regex HrefRegex = new Regex("\\s+href\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");
    private Regex SrcRegex = new Regex("\\s+src\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");
    private Regex ContentRegex = new Regex("\\s+content\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");

    public IList<String> ParserFileForInternalLinks(Uri pageUri, string fileAsString)
    {
        var foundFiles = new List<String>();
        return foundFiles;
    }
}