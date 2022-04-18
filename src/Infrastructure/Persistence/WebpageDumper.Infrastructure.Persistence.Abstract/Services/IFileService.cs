namespace WebpageDumper.Infrastructure.Persistence.Services;

public interface IFileService
{
    void CreateDirectory(String path);
    Task WriteFile(Stream fileData, String fileName);
    Task WriteFileToPathAsync(Task<Stream> fileData, string fileName, string? path = null, string outputDir = "output");
    Task WriteFileToPath(Stream fileData, String fileName, String? path = null, String outputDir = "output");
}