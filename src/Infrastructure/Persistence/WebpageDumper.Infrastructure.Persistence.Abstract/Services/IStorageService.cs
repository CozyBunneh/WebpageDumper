namespace WebpageDumper.Infrastructure.Persistence.Services;

public interface IStorageService
{
    Task WriteFile(Stream fileData, String fileName);
    Task WriteFileToPathAsync(string output, Task<Stream> fileData, string fileName, string? path = null);
    Task WriteFileToPath(string output, Stream fileData, String fileName, String? path = null);
}