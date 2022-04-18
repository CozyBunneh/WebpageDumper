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
            IWebService? webService = args[0] as IWebService;
            IFileService? fileService = args[1] as IFileService;
            IList<WebpageResource>? failedWebpageResources = args[2] as IList<WebpageResource>;
            DownloadWebpageCommand? command = args[3] as DownloadWebpageCommand;
            WebpageResource? webpageResource = args[4] as WebpageResource;
            ProgressBar? progressBar = args[5] as ProgressBar;
            ManualResetEvent? myEvent = (ManualResetEvent)args[6];

            if (webService != null
                && fileService != null
                && failedWebpageResources != null
                && command != null
                && webpageResource != null
                && progressBar != null
                && myEvent != null)
            {
                Task<Stream> fileStream = webService.GetWebpageResourceAsStreamAsync(command.uri, webpageResource);
                try
                {
                    fileService.WriteFileToPathAsync(
                                        command.output,
                                        fileStream,
                                        webpageResource.fileName,
                                        webpageResource.path).Wait();
                }
                catch (AggregateException)
                {
                    failedWebpageResources.Add(webpageResource);
                }
                progressBar.Tick();
                myEvent.Set();
            }
        }
    }
}