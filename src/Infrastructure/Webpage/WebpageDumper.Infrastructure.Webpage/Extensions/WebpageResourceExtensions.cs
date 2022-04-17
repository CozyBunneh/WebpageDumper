using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Infrastructure.Webpage.Extensions;

public static class WebpageResourceExtensions
{
    public static List<WebpageResource> ToDtos(this List<string> httpResourceStrings)
    {
        var dtos = httpResourceStrings.Select(httpResourceString => httpResourceString.ToDto()).ToList();
        dtos.Sort();
        return dtos;
    }

    public static WebpageResource ToDto(this string httpResourceString)
    {
        return new WebpageResource(httpResourceString.GetFileNameOfHttpResourceString(),
                                   httpResourceString.GetDirectoryPathOfHttpResourceString());
    }
}