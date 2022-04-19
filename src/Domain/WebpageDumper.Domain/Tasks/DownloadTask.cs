using ShellProgressBar;
using WebpageDumper.Domain.Abstract.Commands;
using WebpageDumper.Infrastructure.Persistence.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;
using WebpageDumper.Infrastructure.Webpage.Abstract.Service;

namespace WebpageDumper.Domain.Services.Tasks;

public static class DownloadTask
{
    public static void DownloadFileAsync(object? state)
    {
        object[]? args = state as object[];
        if (args != null)
        {
            // Get all arguments from the object array.
            IWebService? webService = args[0] as IWebService;
            IStorageService? storageService = args[1] as IStorageService;
            IList<WebpageResource>? failedWebpageResources = args[2] as IList<WebpageResource>;
            DownloadWebpageCommand? command = args[3] as DownloadWebpageCommand;
            WebpageResource? webpageResource = args[4] as WebpageResource;
            ProgressBar? progressBar = args[5] as ProgressBar;
            ManualResetEvent? myEvent = (ManualResetEvent)args[6];

            if (webService != null
                && storageService != null
                && failedWebpageResources != null
                && command != null
                && webpageResource != null
                && progressBar != null
                && myEvent != null)
            {
                Task<Stream> fileStream = webService.GetWebpageResourceAsStreamAsync(command.uri, webpageResource);
                try
                {
                    storageService.WriteFileToPathAsync(
                        command.output,
                        fileStream,
                        webpageResource.fileName,
                        webpageResource.path).Wait();
                }
                catch (AggregateException)
                {
                    // Catch 404 and add the failed resource into a list so that it can later be displayed.
                    failedWebpageResources.Add(webpageResource);
                }
                progressBar.Tick();
                myEvent.Set();
            }
        }
    }
}