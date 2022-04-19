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
    private const String UnableToDownloadTheFiles = "Unable to download the files:";
    private const ConsoleColor ForegroundColor = ConsoleColor.Magenta;
    private const ConsoleColor BackgroundColor = ConsoleColor.DarkGray;
    private const Char ProgressCharacter = '─';

    private IServiceProvider _serviceProvider;
    private ILogger<ProgressLoaderService> _logger;
    private IWebService _webService;
    private IStorageService _storageService;

    public ProgressLoaderService(
        IServiceProvider serviceProvider,
        ILogger<ProgressLoaderService> logger,
        IWebService webService,
        IStorageService storageService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _webService = webService;
        _storageService = storageService;
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
        ShowFilesThatCouldntBeDownloadedInConsoleOutput(failedWebpageResources);
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
            DownloadTask.DownloadFileAsync,
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
        var webService = _serviceProvider.GetService(typeof(IWebService));
        var storageService = _serviceProvider.GetService(typeof(IStorageService));
        return new object[]
        {
            webService!,
            storageService!,
            failedWebpageResources,
            command,
            webpageResource,
            progressBar,
            myEvent
        };
    }

    private void ShowFilesThatCouldntBeDownloadedInConsoleOutput(IList<WebpageResource> failedWebpageResources)
    {
        if (failedWebpageResources.Count > 0)
        {
            Console.WriteLine(UnableToDownloadTheFiles);
            foreach (var failedWebpageResource in failedWebpageResources)
            {
                Console.WriteLine($"\t{failedWebpageResource.path}/{failedWebpageResource.fileName}");
            }
        }
    }
}