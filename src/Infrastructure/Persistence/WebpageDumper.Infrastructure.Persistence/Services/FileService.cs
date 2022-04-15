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
        throw new NotImplementedException();
    }

    public Task WriteFile(Stream fileData, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task WriteFileToPath(Stream fileData, string fileName, string path)
    {
        throw new NotImplementedException();
    }
}