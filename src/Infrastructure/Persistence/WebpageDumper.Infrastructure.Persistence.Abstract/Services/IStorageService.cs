namespace WebpageDumper.Infrastructure.Persistence.Services;

public interface IStorageService
{
    Task WriteFileToPathAsync(
        string output,
        Task<Stream> fileData,
        string fileName,
        string? path = null);

    Task WriteFileAsStringToPathAsync(
        string output,
        Task<String> fileData,
        string fileName,
        string? path = null);
}