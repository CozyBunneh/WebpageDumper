namespace WebpageDumper.Infrastructure.External.Abstract.Service;

public interface IWebService
{
    Task<Stream> GetFileStreamAsync(Uri uri);
}