namespace WebpageDumper.Infrastructure.External.Extensions;

public static class HttpClientExtensions
{
    public static Task DownloadFileStreamAsync(this HttpClient client, Uri uri)
    {
        return client.GetStreamAsync(uri);
    }
}