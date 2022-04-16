namespace WebpageDumper.Infrastructure.Webpage.Extensions;

public static class StringExtensions
{
    public static String SanitizeHttpResourceString(this String httpResourceString, List<String> leadingStringsToRemove)
    {
        httpResourceString = httpResourceString.Trim();

        foreach (var leadingStringToRemove in leadingStringsToRemove)
        {
            httpResourceString = httpResourceString.Replace(leadingStringToRemove, "");
        }

        return httpResourceString.RemoveQuotationMarks().TrimLeadingFrontSlash();
    }

    public static bool IsFile(this String httpResourceString)
    {
        var lastPartOfMatch = GetFileNameOfHttpResourceString(httpResourceString);
        return lastPartOfMatch.Contains('.');
    }

    public static String RemoveQuotationMarks(this String httpResourceString)
    {
        return httpResourceString.Replace("\"", "");
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