using Microsoft.Extensions.Logging;
using ShellProgressBar;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Domain.Services.Tasks;
using WebpageDumper.Infrastructure.Persistence.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;
using WebpageDumper.Infrastructure.Webpage.Abstract.Service;

namespace WebpageDumper.Domain.Services;

public class ProgressLoaderService : IProgressLoaderService
{
    private const String StartingDownloadOfResourcesUsing = "Starting download of resources using ";
    private const String Threads = " threads...";
    private const String DownloadingResourcesFrom = "Downloading resources from: ";
    private const ConsoleColor ForegroundColor = ConsoleColor.Cyan;
    private const ConsoleColor BackgroundColor = ConsoleColor.DarkGray;
    private const Char ProgressCharacter = '-';

    private ILogger<ProgressLoaderService> _logger;
    private IWebService _webService;
    private IFileService _fileService;

    public ProgressLoaderService(
        ILogger<ProgressLoaderService> logger,
        IWebService webService,
        IFileService fileService)
    {
        _logger = logger;
        _webService = webService;
        _fileService = fileService;
    }

    public async Task DownloadWebpageResources(Uri uri, int numberOfThreads, IList<WebpageResource> webpageResources)
    {
        Console.WriteLine($"{StartingDownloadOfResourcesUsing}{numberOfThreads}{Threads}");

        var options = new ProgressBarOptions
        {
            ForegroundColor = ForegroundColor,
            BackgroundColor = BackgroundColor,
            ProgressCharacter = ProgressCharacter
        };

        using (var progressBar = new ProgressBar(
            webpageResources.Count,
            $"{DownloadingResourcesFrom}{uri.ToString()}",
            options))
        {
            RunAllDownloadThreads(
                uri,
                webpageResources,
                numberOfThreads,
                webpageResources.Count,
                progressBar);
        }
    }

    private void RunAllDownloadThreads(
        Uri uri,
        IList<WebpageResource> webpageResources,
        int numberOfThreads,
        int numberOfTasks,
        ProgressBar progressBar)
    {
        var events = new ManualResetEvent[numberOfTasks];
        ThreadPool.SetMaxThreads(numberOfThreads, numberOfThreads);
        for (int i = 0; i < numberOfTasks; i++)
        {
            events[i] = QueueDownloadThreadForWebpageResource(uri, webpageResources[i], progressBar);
        }
        WaitHandle.WaitAll(events);
    }

    private ManualResetEvent QueueDownloadThreadForWebpageResource(
        Uri uri,
        WebpageResource webpageResource,
        ProgressBar progressBar)
    {
        var myEvent = new ManualResetEvent(false);
        ThreadPool.QueueUserWorkItem(
            new WaitCallback(DownloadTask.DownloadFileAsync),
            GetDownloadThreadObjectArray(
                uri,
                webpageResource,
                progressBar,
                myEvent));
        return myEvent;
    }

    private object[] GetDownloadThreadObjectArray(
        Uri uri,
        WebpageResource webpageResource,
        ProgressBar progressBar,
        ManualResetEvent myEvent)
    {
        return new object[]
        {
            _webService,
            _fileService,
            uri,
            webpageResource,
            progressBar,
            myEvent
        };
    }
}