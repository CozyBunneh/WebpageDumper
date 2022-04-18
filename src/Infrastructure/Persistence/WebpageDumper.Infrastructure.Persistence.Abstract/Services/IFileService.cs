namespace WebpageDumper.Infrastructure.Persistence.Services;

public interface IFileService
{
    Task WriteFile(Stream fileData, String fileName);
    Task WriteFileToPathAsync(string outputDir, Task<Stream> fileData, string fileName, string? path = null);
    Task WriteFileToPath(string outputDir, Stream fileData, String fileName, String? path = null);
}