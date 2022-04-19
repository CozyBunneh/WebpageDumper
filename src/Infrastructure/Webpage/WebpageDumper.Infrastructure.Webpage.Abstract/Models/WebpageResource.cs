namespace WebpageDumper.Infrastructure.Webpage.Abstract.Models;

public record WebpageResource(String fileName, String path) : IComparable<WebpageResource>
{
    private static readonly string[] CommonVideoExtensions =
    {
        "264",
        "3g2",
        "3gp",
        "arf",
        "asf",
        "asx",
        "avi",
        "bik",
        "dash",
        "dat",
        "flv",
        "h264",
        "m2t",
        "m2ts",
        "mv4",
        "mkv",
        "mod",
        "mov",
        "mp4",
        "mpeg",
        "mpeg",
        "mpg",
        "mts",
        "ogv",
        "rec",
        "rmvb",
        "swf",
        "webm",
        "wmv"
    };

    private static readonly string[] CommonTextFileExtensions =
    {
        "1st",
        "asp",
        "csv",
        "htm",
        "html",
        "log",
        "lrc",
        "lst",
        "md",
        "nfo",
        "opml",
        "plist",
        "rtf",
        "srt",
        "sub",
        "tbl",
        "text",
        "txt",
        "xml",
        "xmlp",
        "xmp",
        "xsl",
        "xslt",
        "js",
        "ts",
        "css",
        "scss",
        "svg",
    };

    public bool IsTextFile() => CommonTextFileExtensions.Contains(GetFileExtension());

    public int CompareTo(WebpageResource? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (CommonVideoExtensions.Contains(this.GetFileExtension()))
        {
            if (CommonVideoExtensions.Contains(other.GetFileExtension()))
            {
                return this.fileName.CompareTo(other.fileName);
            }
            return -1;
        }
        else
        {
            if (CommonVideoExtensions.Contains(other.GetFileExtension()))
            {
                return 1;
            }
            return this.fileName.CompareTo(other.fileName);
        }
    }

    public String GetFileExtension()
    {
        return this.fileName.Split('.').Last();
    }
}