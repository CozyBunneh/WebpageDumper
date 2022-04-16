using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Infrastructure.Webpage.Extensions;

public static class WebpageResourceExtensions
{
    public static List<WebpageResource> ToDtos(this List<string> httpResourceStrings)
    {
        return httpResourceStrings.Select(httpResourceString => httpResourceString.ToDto()).ToList();
    }

    public static WebpageResource ToDto(this string httpResourceString)
    {
        return new WebpageResource(httpResourceString.GetFileNameOfHttpResourceString(),
                                   httpResourceString.GetDirectoryPathOfHttpResourceString());
    }
}