using Microsoft.Extensions.Logging;

namespace WebpageDumper.Infrastructure.Persistence.Services;

public class FileService : IFileService
{
    private ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
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
        string outputDir,
        Task<Stream> fileData,
        string fileName,
        string? path = null)
    {
        await WriteFileToPath(outputDir, await fileData, fileName, path);
    }

    public Task WriteFileToPath(
        string outputDir,
        Stream fileData,
        string fileName,
        string? path = null)
    {
        var fileNameWithFullPath = CreateDirectoryAndReturnFullFilenamePath(outputDir, fileName, path);
        return WriteFile(fileData, fileNameWithFullPath);
    }

    private String CreateDirectoryAndReturnFullFilenamePath(
        string outputDir,
        string fileName,
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

    private void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }
}