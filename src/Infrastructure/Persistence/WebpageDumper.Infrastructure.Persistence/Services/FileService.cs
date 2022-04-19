using Microsoft.Extensions.Logging;

namespace WebpageDumper.Infrastructure.Persistence.Services;

public class FileService : IStorageService
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

    /// <summary>
    ///     Pipe a file stream task to a file writer in an asynchronous manner.
    /// </summary>
    /// <param name="output"></param>
    /// <param name="fileData"></param>
    /// <param name="fileName"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task WriteFileToPathAsync(
        string output,
        Task<Stream> fileData,
        string fileName,
        string? path = null)
    {
        await WriteFileToPath(output, await fileData, fileName, path);
    }

    /// <summary>
    ///     Write an already synchronously fetched file stream to a file.
    /// </summary>
    /// <param name="output"></param>
    /// <param name="fileData"></param>
    /// <param name="fileName"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public Task WriteFileToPath(
        string output,
        Stream fileData,
        string fileName,
        string? path = null)
    {
        var fileNameWithFullPath = CreateDirectoryAndReturnFullFilenamePath(output, fileName, path);
        return WriteFile(fileData, fileNameWithFullPath);
    }

    private String CreateDirectoryAndReturnFullFilenamePath(
        string output,
        string fileName,
        string? path = null)
    {
        if (!String.IsNullOrEmpty(path))
        {
            CreateDirectory($"{output}/{path}");
            fileName = $"{output}/{path}/{fileName}";
        }
        else
        {
            CreateDirectory($"{output}");
            fileName = $"{output}/{fileName}";
        }

        return fileName;
    }

    private void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }
}