using Microsoft.Extensions.Logging;

namespace WebpageDumper.Infrastructure.Persistence.Services;

public class FileService : IFileService
{
    private const String DefaultOutputDirectory = "output";

    private ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    public Task WriteFile(Stream fileData, string fileName)
    {
        try
        {
            var fs = new FileStream(fileName, FileMode.CreateNew);
            return fileData.CopyToAsync(fs);
        }
        catch (IOException)
        {
            // Simply ignore the error since this means that the file already exists.
            return Task.CompletedTask;
        }
    }

    public async Task WriteFileToPathAsync(
        Task<Stream> fileData,
        string fileName,
        string? path = null,
        string outputDir = DefaultOutputDirectory)
    {
        await WriteFileToPath(await fileData, fileName, path, outputDir);
    }

    public Task WriteFileToPath(
        Stream fileData,
        string fileName,
        string? path = null,
        string outputDir = DefaultOutputDirectory)
    {
        var fileNameWithFullPath = CreateDirectoryAndReturnFullFilenamePath(fileName, outputDir, path);
        return WriteFile(fileData, fileNameWithFullPath);
    }

    private String CreateDirectoryAndReturnFullFilenamePath(
        string fileName,
        string outputDir,
        string? path = null)
    {
        if (!String.IsNullOrEmpty(path))
        {
            CreateDirectory($"{outputDir}/{path}");
            fileName = $"{outputDir}/{path}/{fileName}";
        }
        else
        {
            CreateDirectory($"{outputDir}");
            fileName = $"{outputDir}/{fileName}";
        }

        return fileName;
    }
}