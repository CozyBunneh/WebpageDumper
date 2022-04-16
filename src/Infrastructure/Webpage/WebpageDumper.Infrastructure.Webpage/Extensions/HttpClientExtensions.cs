namespace WebpageDumper.Infrastructure.Webpage.Extensions;

public static class HttpClientExtensions
{
    public static Task DownloadFileStreamAsync(this HttpClient client, Uri uri)
    {
        return client.GetStreamAsync(uri);
    }
}