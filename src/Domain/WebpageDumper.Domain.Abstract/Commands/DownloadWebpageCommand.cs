namespace WebpageDumper.Domain.Abstract.Commands;

public record DownloadWebpageCommand(Uri uri, int numberOfThreads, string output);
