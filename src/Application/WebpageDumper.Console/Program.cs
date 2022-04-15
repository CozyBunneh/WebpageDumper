﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebpageDumper.Application.WebpageDumper.Console.HostedService;
using WebpageDumper.Infrastructure.Domain.Configuration;
using WebpageDumper.Infrastructure.External.Configuration;
using WebpageDumper.Infrastructure.Persistence.Configuration;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureLogging(logging =>
{
    logging.AddConsole();
});
builder.UseConsoleLifetime(consoleBuilder =>
{
    consoleBuilder.SuppressStatusMessages = true;
});
var host = builder.ConfigureServices(services =>
{
    services.AddLogging(logginBuilder => logginBuilder.AddConsole());
    services.AddPersistenceServices();
    services.AddExternalServices();
    services.AddDomainServices();
    services.AddHostedService<ConsoleService>();
}).Build();

await host.RunAsync();
