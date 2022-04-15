using Microsoft.Extensions.Logging;

namespace WebpageDumper.Infrastructure.Persistence.Services;

public class FileService : IFileService
{
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
        var fs = new FileStream(fileName, FileMode.CreateNew);
        return fileData.CopyToAsync(fs);
    }

    public Task WriteFileToPath(Stream fileData, string fileName, string? path = null)
    {
        if (path != null)
        {
            CreateDirectory(path);
            fileName = $"{path}/{fileName}";
        }

        return WriteFile(fileData, fileName);
    }
}