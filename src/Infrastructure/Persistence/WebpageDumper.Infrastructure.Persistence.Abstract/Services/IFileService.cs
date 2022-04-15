namespace WebpageDumper.Infrastructure.Persistence.Services;

public interface IFileService
{
    void CreateDirectory(String path);
    Task WriteFile(Stream fileData, String fileName);
    Task WriteFileToPath(Stream fileData, String fileName, String path);
}