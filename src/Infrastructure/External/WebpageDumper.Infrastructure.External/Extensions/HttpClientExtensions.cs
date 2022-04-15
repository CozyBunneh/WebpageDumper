namespace WebpageDumper.Infrastructure.External.Extensions;

public static class HttpClientExtensions
{
    public static Task DownloadFileStreamAsync(this HttpClient client, Uri uri)
    {
        return client.GetStreamAsync(uri);
    }

    // public static async Task DownloadFileToPathAsync(this HttpClient client, Uri uri, string fileName, string path = null)
    // {
    //     using (var s = await client.GetStreamAsync(uri))
    //     {
    //         if (path != null)
    //         {
    //             Directory.CreateDirectory(path);
    //             fileName = $"{path}/{fileName}";
    //         }

    //         using (var fs = new FileStream(fileName, FileMode.CreateNew))
    //         {
    //             await s.CopyToAsync(fs);
    //         }
    //     }
    // }
}