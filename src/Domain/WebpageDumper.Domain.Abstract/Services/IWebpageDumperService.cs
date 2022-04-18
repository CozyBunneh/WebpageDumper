using WebpageDumper.Domain.Abstract.Commands;

namespace WebpageDumper.Domain.Abstract.Services;

public interface IWebpageDumperService
{
    Task DumpWebpage(DownloadWebpageCommand command);
}