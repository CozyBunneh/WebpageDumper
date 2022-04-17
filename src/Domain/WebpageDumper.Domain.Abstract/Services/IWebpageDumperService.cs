namespace WebpageDumper.Domain.Abstract.Services;

public interface IWebpageDumperService
{
    Task DumpWebpage(Uri uri, int numberOfThreads = 4);
}