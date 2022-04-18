namespace WebpageDumper.Infrastructure.Webpage.Extensions;

public static class StringExtensions
{
    private const String Http = "http";
    private const String Www = "www";

    public static String SanitizeHttpResourceString(
        this String httpResourceString,
        List<String> leadingStringsToRemove)
    {
        httpResourceString = httpResourceString.Trim();

        foreach (var leadingStringToRemove in leadingStringsToRemove)
        {
            httpResourceString = httpResourceString.Replace(leadingStringToRemove, "");
        }

        return httpResourceString.RemoveQuotationMarks().TrimAllLeadingFrontSlashes();
    }

    public static bool IsFile(this String httpResourceString)
    {
        // If a resource starts with http or www after the parsing then it's not a local resource file.
        if (httpResourceString.StartsWith(Http) || httpResourceString.StartsWith(Www))
        {
            return false;
        }

        var lastPartOfMatch = GetFileNameOfHttpResourceString(httpResourceString);
        return lastPartOfMatch.Contains('.');
    }

    public static String RemoveQuotationMarks(this String httpResourceString)
    {
        return httpResourceString.Replace("\"", "");
    }

    public static String TrimAllLeadingFrontSlashes(this String httpResourceString)
    {
        while (httpResourceString.IndexOf('/') == 0)
        {
            httpResourceString = httpResourceString.TrimLeadingFrontSlash();
        }

        return httpResourceString;
    }

    public static String TrimLeadingFrontSlash(this String httpResourceString)
    {
        if (httpResourceString.IndexOf('/') == 0)
        {
            httpResourceString = httpResourceString.Substring(1);
        }

        return httpResourceString;
    }

    public static String GetFileNameOfHttpResourceString(this String httpResourceString)
    {
        if (httpResourceString.Contains('/'))
        {
            var matchParts = httpResourceString.Split('/');
            return matchParts.Last();
        }

        return httpResourceString;
    }

    public static String GetDirectoryPathOfHttpResourceString(this String httpResourceString)
    {
        if (httpResourceString.Contains('/'))
        {
            var matchParts = httpResourceString.Split('/');
            return string.Join('/', matchParts[..^1]);
        }

        return "";
    }
}