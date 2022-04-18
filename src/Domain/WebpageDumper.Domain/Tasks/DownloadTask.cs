using ShellProgressBar;
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
            Uri? uri = args[2] as Uri;
            WebpageResource? webpageResource = args[3] as WebpageResource;
            ProgressBar? progressBar = args[4] as ProgressBar;

            if (webService != null
                && fileService != null
                && uri != null
                && webpageResource != null
                && progressBar != null
                && args[5] != null)
            {
                Task<Stream> fileStream = webService.GetWebpageResourceAsStreamAsync(uri, webpageResource);
                fileService.WriteFileToPathAsync(fileStream, webpageResource.fileName, webpageResource.path).Wait();
                progressBar.Tick();
                (args[5] as ManualResetEvent).Set();
            }
        }
    }
}