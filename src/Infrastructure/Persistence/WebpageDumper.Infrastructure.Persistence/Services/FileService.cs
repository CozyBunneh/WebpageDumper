using Microsoft.Extensions.Logging;

namespace WebpageDumper.Infrastructure.Persistence.Services;

public class FileService : IStorageService
{
    private ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
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
        var fileNameWithFullPath = CreateDirectoryAndReturnFullFilenamePath(output, fileName, path);
        await WriteFileAsync(await fileData, fileNameWithFullPath);
    }

    public async Task WriteFileAsStringToPathAsync(
        string output,
        Task<String> fileData,
        string fileName,
        string? path = null)
    {
        var fileNameWithFullPath = CreateDirectoryAndReturnFullFilenamePath(output, fileName, path);
        String file = await fileData;
        WriteFileAsString(file, fileNameWithFullPath);
    }

    private Task WriteFileAsync(Stream fileData, string fileName)
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

    private void WriteFile(Stream fileData, string fileName)
    {
        try
        {
            var fs = new FileStream(fileName, FileMode.CreateNew);
            fileData.CopyToAsync(fs);
        }
        catch (IOException)
        {
            // Simply ignore the error since this means that the file already exists.
        }
    }

    private void WriteFileAsString(String fileData, string fileName)
    {
        try
        {
            var fileWriter = new System.IO.StreamWriter(fileName);
            fileWriter.WriteLine(fileData);
            fileWriter.Dispose();
        }
        catch (IOException)
        {
            // Simply ignore the error since this means that the file already exists.
        }
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