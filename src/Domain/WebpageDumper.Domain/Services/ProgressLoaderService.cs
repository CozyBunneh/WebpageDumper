using Microsoft.Extensions.Logging;
using ShellProgressBar;
using WebpageDumper.Domain.Abstract.Commands;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Domain.Services.Tasks;
using WebpageDumper.Infrastructure.Persistence.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;
using WebpageDumper.Infrastructure.Webpage.Abstract.Service;

namespace WebpageDumper.Domain.Services;

public class ProgressLoaderService : IProgressLoaderService
{
    private const String StartingDownloadOfResourcesUsing = "Starting download of resources using ";
    private const String ThreadsToOutput = " threads to the output folder: ";
    private const String DownloadingResourcesFrom = "Downloading resources from: ";
    private const ConsoleColor ForegroundColor = ConsoleColor.Magenta;
    private const ConsoleColor BackgroundColor = ConsoleColor.DarkGray;
    private const Char ProgressCharacter = '─';

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

    public void DownloadWebpageResources(
        DownloadWebpageCommand command,
        IList<WebpageResource> webpageResources)
    {
        Console.WriteLine($"{StartingDownloadOfResourcesUsing}{command.numberOfThreads}{ThreadsToOutput}{command.output}");

        var options = new ProgressBarOptions
        {
            ForegroundColor = ForegroundColor,
            BackgroundColor = BackgroundColor,
            ProgressCharacter = ProgressCharacter
        };

        var failedWebpageResources = new List<WebpageResource>();
        using (var progressBar = new ProgressBar(
            webpageResources.Count,
            $"{DownloadingResourcesFrom}{command.uri.ToString()}",
            options))
        {
            RunAllDownloadThreads(
                command,
                failedWebpageResources,
                webpageResources,
                webpageResources.Count,
                progressBar);
        }
        if (failedWebpageResources.Count > 0)
        {
            Console.WriteLine("Unable to download the files:");
            foreach (var failedWebpageResource in failedWebpageResources)
            {
                Console.WriteLine($"\t{failedWebpageResource.path}/{failedWebpageResource.fileName}");
            }
        }
    }

    private void RunAllDownloadThreads(
        DownloadWebpageCommand command,
        IList<WebpageResource> failedWebpageResources,
        IList<WebpageResource> webpageResources,
        int numberOfTasks,
        ProgressBar progressBar)
    {
        var events = new ManualResetEvent[numberOfTasks];
        ThreadPool.SetMaxThreads(command.numberOfThreads, command.numberOfThreads);
        for (int i = 0; i < numberOfTasks; i++)
        {
            events[i] = QueueDownloadThreadForWebpageResource(
                command,
                failedWebpageResources,
                webpageResources[i],
                progressBar);
        }
        WaitHandle.WaitAll(events);
    }

    private ManualResetEvent QueueDownloadThreadForWebpageResource(
        DownloadWebpageCommand command,
        IList<WebpageResource> failedWebpageResources,
        WebpageResource webpageResource,
        ProgressBar progressBar)
    {
        var myEvent = new ManualResetEvent(false);
        ThreadPool.QueueUserWorkItem(
            new WaitCallback(DownloadTask.DownloadFileAsync),
            GetDownloadThreadObjectArray(
                command,
                failedWebpageResources,
                webpageResource,
                progressBar,
                myEvent));
        return myEvent;
    }

    private object[] GetDownloadThreadObjectArray(
        DownloadWebpageCommand command,
        IList<WebpageResource> failedWebpageResources,
        WebpageResource webpageResource,
        ProgressBar progressBar,
        ManualResetEvent myEvent)
    {
        return new object[]
        {
            _webService,
            _fileService,
            failedWebpageResources,
            command,
            webpageResource,
            progressBar,
            myEvent
        };
    }
}