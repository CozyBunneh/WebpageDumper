using WebpageDumper.Domain.Abstract.Commands;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Domain.Abstract.Services;

public interface IProgressLoaderService
{
    void DownloadWebpageResources(DownloadWebpageCommand command, IList<WebpageResource> webpageResources);
}