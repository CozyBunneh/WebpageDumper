using CommandLine;

namespace WebpageDumper.Console.Models;

public class CommandLineOptions
{
    private const String Http = "http";
    private const String Https = "https";
    private const String ColonSlashSlash = "://";

    private string? _webpageAddress;
    [Option(shortName: 'a', longName: "address", Required = true, HelpText = "Address to the webpage to dump all files from.\nIf https/http isn't provided it defaults to https.")]
    public string? WebpageAddress
    {
        get
        {
            if (_webpageAddress != null && !_webpageAddress.Contains(Http))
            {
                return $"{Https}{ColonSlashSlash}{_webpageAddress}";
            }
            return _webpageAddress;
        }
        set { _webpageAddress = value; }
    }

    [Option(shortName: 't', longName: "threads", Required = false, HelpText = "Number of download threads to use.", Default = 4)]
    public int Threads { get; set; }

    [Option(shortName: 'o', longName: "output", Required = false, HelpText = "Output folder to download the webpage files into.", Default = "output")]
    public string? Output { get; set; }
}