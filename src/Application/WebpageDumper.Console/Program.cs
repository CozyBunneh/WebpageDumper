using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WebpageDumper.Application.WebpageDumper.Console;

class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).ConfigureAppConfiguration((hostContext, builder) =>
        { }).Build();
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
}