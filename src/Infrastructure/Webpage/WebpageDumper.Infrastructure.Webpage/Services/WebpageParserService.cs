using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;
using WebpageDumper.Infrastructure.Webpage.Abstract.Services;
using WebpageDumper.Infrastructure.Webpage.Extensions;

namespace WebpageDumper.Infrastructure.Webpage.Services;

public class WebpageParserService : IWebpageParserService
{
    private static readonly Regex HrefRegex = new Regex("\\s+href\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");
    private static readonly Regex SrcRegex = new Regex("\\s+src\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");
    private static readonly Regex ContentRegex = new Regex("\\s+content\\=\"[\\/\\w\\d\\.\\-\\?\\:]*\"");
    private const String Href = "href=";
    private const String Src = "src=";
    private const String Content = "content=";
    private const String Www = "www";
    private const String IndexHtml = "index.html";

    private ILogger<WebpageParserService> _logger;
    private List<String> _leadingStringsToRemove;

    public WebpageParserService(ILogger<WebpageParserService> logger)
    {
        _logger = logger;
        _leadingStringsToRemove = new List<String>() { Href, Src, Content };
    }

    public IList<WebpageResource> ParseWebpageForResources(Uri pageUri, string fileAsString)
    {
        AddWebpageUriToLeadingStringsToRemoveFromFileResourcePaths(pageUri);
        var foundFiles = new List<String>();

        foundFiles.AddRange(GetRegExMatches(HrefRegex, fileAsString));
        foundFiles.AddRange(GetRegExMatches(SrcRegex, fileAsString));
        foundFiles.AddRange(GetRegExMatches(ContentRegex, fileAsString));
        foundFiles.Add(IndexHtml);

        return foundFiles.Distinct().ToList().ToDtos();
    }

    private void AddWebpageUriToLeadingStringsToRemoveFromFileResourcePaths(Uri pageUri)
    {
        _leadingStringsToRemove.Add(pageUri.ToString());
        if (!IsWwwInUri(pageUri))
        {
            string wwwPageUri = $"{pageUri.Scheme}://{Www}.{pageUri.Host}";
            _leadingStringsToRemove.Add(wwwPageUri);
        }
    }

    private List<String> GetRegExMatches(Regex regex, String fileAsString)
    {
        return (from Match match in regex.Matches(fileAsString)
                let httpResourceString = match.Value.SanitizeHttpResourceString(_leadingStringsToRemove)
                where httpResourceString.IsFile()
                select httpResourceString).ToList();
    }

    private bool IsWwwInUri(Uri uri)
    {
        return uri.Host.Contains(Www);
    }
}