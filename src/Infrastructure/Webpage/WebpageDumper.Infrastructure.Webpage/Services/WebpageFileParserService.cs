using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using WebpageDumper.Infrastructure.Webpage.Abstract.Services;
using WebpageDumper.Infrastructure.Webpage.Extensions;

namespace WebpageDumper.Infrastructure.Webpage.Services;

public class WebpageFileParserService : IWebpageFileParserService
{
    private static readonly Regex HrefRegex = new Regex("\\s+href\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");
    private static readonly Regex SrcRegex = new Regex("\\s+src\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");
    private static readonly Regex ContentRegex = new Regex("\\s+content\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");
    private static readonly String Href = "href=";
    private static readonly String Src = "src=";
    private static readonly String Content = "content=";

    private ILogger<WebpageFileParserService> _logger;
    private List<String> _leadingStringsToRemove;

    public WebpageFileParserService(ILogger<WebpageFileParserService> logger)
    {
        _logger = logger;
        _leadingStringsToRemove = new List<String>() { Href, Src, Content };
    }

    public IList<String> ParserFileForInternalLinks(Uri pageUri, string fileAsString)
    {
        // Add the page uri since we want to trim this from the file results
        _leadingStringsToRemove.Add(pageUri.ToString());

        var foundFiles = new List<String>();
        foundFiles.AddRange(GetRegExMatches(HrefRegex, fileAsString));
        foundFiles.AddRange(GetRegExMatches(SrcRegex, fileAsString));
        foundFiles.AddRange(GetRegExMatches(ContentRegex, fileAsString));

        return foundFiles;
    }

    private List<String> GetRegExMatches(Regex regex, String fileAsString)
    {
        return (from Match match in regex.Matches(fileAsString)
                let httpResourceString = match.Value.SanitizeHttpResourceString(_leadingStringsToRemove)
                where httpResourceString.IsFile()
                select httpResourceString).ToList();
    }
}