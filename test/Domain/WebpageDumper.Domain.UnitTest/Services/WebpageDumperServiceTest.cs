using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Extensions;
using NUnit.Framework;
using WebpageDumper.Domain.Abstract.Commands;
using WebpageDumper.Domain.Abstract.Services;
using WebpageDumper.Domain.Services;
using WebpageDumper.Infrastructure.Webpage.Abstract.Models;

namespace WebpageDumper.Domain.UnitTest.Services;

[TestFixture]
public class WebpageDumperServiceTest
{
    private Fixture? _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Test]
    public async Task Dump_Webpage_When_Index_Under_Given_NonWwwUri_Successfull()
    {
        // Arrange
        var loggerSubstitute = Substitute.For<ILogger<WebpageDumperService>>();
        var spinnerLoaderServiceSubstitute = Substitute.For<ISpinnerLoaderService>();
        var progressLoaderServiceSubstitute = Substitute.For<IProgressLoaderService>();
        var sut = new WebpageDumperService(loggerSubstitute, spinnerLoaderServiceSubstitute, progressLoaderServiceSubstitute);

        var uri = new UriBuilder("https://google.com").Uri;
        var command = new DownloadWebpageCommand(uri, 4, "output");
        var htmlFileAsString = $"<!DOCTYPE html><html lang=\"en\" xmlns:fb=\"http://somelink.com\"><head><link rel=\"prefetch\" href=\"/assets/folder1/file1.jpg\" /><script src=\"/assets/js/script1.js\"></script></body></html>";
        var webpageResources = new List<WebpageResource>() {
            new WebpageResource("file1.jpg", "assets/folder1"),
            new WebpageResource("script1.js", "assets/js"),
        };
        spinnerLoaderServiceSubstitute.GetWebpageIndex(uri).Returns(htmlFileAsString);
        spinnerLoaderServiceSubstitute.GetWebresourcesLinksFromIndexPage(
            uri,
            htmlFileAsString).Returns(webpageResources);
        progressLoaderServiceSubstitute
            .When(x => x.DownloadWebpageResources(command, webpageResources))
            .Do(x => { return; });

        // Act
        await sut.DumpWebpage(command);

        // Assert
        await spinnerLoaderServiceSubstitute.Received().GetWebpageIndex(uri);
        spinnerLoaderServiceSubstitute.Received().GetWebresourcesLinksFromIndexPage(
            uri,
            htmlFileAsString);
        progressLoaderServiceSubstitute.Received().DownloadWebpageResources(
            command,
            webpageResources);
    }

    [Test]
    public async Task Dump_Webpage_When_NoIndex_Under_Given_NonWwwUri_Successfull()
    {
        // Arrange
        var loggerSubstitute = Substitute.For<ILogger<WebpageDumperService>>();
        var spinnerLoaderServiceSubstitute = Substitute.For<ISpinnerLoaderService>();
        var progressLoaderServiceSubstitute = Substitute.For<IProgressLoaderService>();
        var sut = new WebpageDumperService(loggerSubstitute, spinnerLoaderServiceSubstitute, progressLoaderServiceSubstitute);

        var uri = new UriBuilder("https://google.com").Uri;
        var command = new DownloadWebpageCommand(uri, 4, "output");
        var htmlFileAsString = "";
        var webpageResources = new List<WebpageResource>() {
            new WebpageResource("file1.jpg", "assets/folder1"),
            new WebpageResource("script1.js", "assets/js"),
        };
        spinnerLoaderServiceSubstitute.GetWebpageIndex(uri).Returns(htmlFileAsString);
        spinnerLoaderServiceSubstitute.GetWebresourcesLinksFromIndexPage(
            uri,
            htmlFileAsString).Returns(webpageResources);
        progressLoaderServiceSubstitute
            .When(x => x.DownloadWebpageResources(command, webpageResources))
            .Do(x => { return; });

        // Act
        await sut.DumpWebpage(command);

        // Assert
        await spinnerLoaderServiceSubstitute.Received().GetWebpageIndex(uri);
        spinnerLoaderServiceSubstitute.DidNotReceive().GetWebresourcesLinksFromIndexPage(
            uri,
            htmlFileAsString);
        progressLoaderServiceSubstitute.DidNotReceive().DownloadWebpageResources(
            command,
            webpageResources);
    }

    [Test]
    public async Task Dump_Webpage_When_Index_UnderWww_Given_NonWwwUri_Successfull()
    {
        // Arrange
        var loggerSubstitute = Substitute.For<ILogger<WebpageDumperService>>();
        var spinnerLoaderServiceSubstitute = Substitute.For<ISpinnerLoaderService>();
        var progressLoaderServiceSubstitute = Substitute.For<IProgressLoaderService>();
        var sut = new WebpageDumperService(loggerSubstitute, spinnerLoaderServiceSubstitute, progressLoaderServiceSubstitute);

        var uri = new UriBuilder("https://google.com").Uri;
        var wwwUri = new UriBuilder("https://www.google.com").Uri;
        var command = new DownloadWebpageCommand(uri, 4, "output");
        var wwwCommand = new DownloadWebpageCommand(wwwUri, 4, "output");
        var htmlFileAsString = $"<!DOCTYPE html><html lang=\"en\" xmlns:fb=\"http://somelink.com\"><head><link rel=\"prefetch\" href=\"/assets/folder1/file1.jpg\" /><script src=\"/assets/js/script1.js\"></script></body></html>";
        var webpageResources = new List<WebpageResource>() {
            new WebpageResource("file1.jpg", "assets/folder1"),
            new WebpageResource("script1.js", "assets/js"),
        };
        spinnerLoaderServiceSubstitute.GetWebpageIndex(uri)
            .Returns<Task<String>>(x => { throw new AggregateException(); });
        spinnerLoaderServiceSubstitute.Configure().GetWebpageIndex(wwwUri).Returns(htmlFileAsString);
        spinnerLoaderServiceSubstitute.GetWebresourcesLinksFromIndexPage(
            wwwUri,
            htmlFileAsString).Returns(webpageResources);
        progressLoaderServiceSubstitute
            .When(x => x.DownloadWebpageResources(wwwCommand, webpageResources))
            .Do(x => { return; });

        // Act
        await sut.DumpWebpage(command);

        // Assert
        await spinnerLoaderServiceSubstitute.Received().GetWebpageIndex(uri);
        await spinnerLoaderServiceSubstitute.Received().GetWebpageIndex(wwwUri);
        spinnerLoaderServiceSubstitute.Received().GetWebresourcesLinksFromIndexPage(
            wwwUri,
            htmlFileAsString);
        progressLoaderServiceSubstitute.Received().DownloadWebpageResources(
            wwwCommand,
            webpageResources);
    }

    [Test]
    public async Task Dump_Webpage_When_Index_Under_Given_WwwUri_Successfull()
    {
        // Arrange
        var loggerSubstitute = Substitute.For<ILogger<WebpageDumperService>>();
        var spinnerLoaderServiceSubstitute = Substitute.For<ISpinnerLoaderService>();
        var progressLoaderServiceSubstitute = Substitute.For<IProgressLoaderService>();
        var sut = new WebpageDumperService(loggerSubstitute, spinnerLoaderServiceSubstitute, progressLoaderServiceSubstitute);

        var uri = new UriBuilder("https://google.com").Uri;
        var wwwUri = new UriBuilder("https://www.google.com").Uri;
        var command = new DownloadWebpageCommand(uri, 4, "output");
        var wwwCommand = new DownloadWebpageCommand(wwwUri, 4, "output");
        var htmlFileAsString = $"<!DOCTYPE html><html lang=\"en\" xmlns:fb=\"http://somelink.com\"><head><link rel=\"prefetch\" href=\"/assets/folder1/file1.jpg\" /><script src=\"/assets/js/script1.js\"></script></body></html>";
        var webpageResources = new List<WebpageResource>() {
            new WebpageResource("file1.jpg", "assets/folder1"),
            new WebpageResource("script1.js", "assets/js"),
        };
        spinnerLoaderServiceSubstitute.GetWebpageIndex(wwwUri)
            .Returns<Task<String>>(x => { throw new AggregateException(); });
        spinnerLoaderServiceSubstitute.Configure().GetWebpageIndex(uri).Returns(htmlFileAsString);
        spinnerLoaderServiceSubstitute.GetWebresourcesLinksFromIndexPage(
            uri,
            htmlFileAsString).Returns(webpageResources);
        progressLoaderServiceSubstitute
            .When(x => x.DownloadWebpageResources(command, webpageResources))
            .Do(x => { return; });

        // Act
        await sut.DumpWebpage(wwwCommand);

        // Assert
        await spinnerLoaderServiceSubstitute.Received().GetWebpageIndex(wwwUri);
        await spinnerLoaderServiceSubstitute.Received().GetWebpageIndex(uri);
        spinnerLoaderServiceSubstitute.Received().GetWebresourcesLinksFromIndexPage(
            uri,
            htmlFileAsString);
        progressLoaderServiceSubstitute.Received().DownloadWebpageResources(
            command,
            webpageResources);
    }

    [Test]
    public async Task Dump_Webpage_When_NoWebpageResources()
    {
        // Arrange
        var loggerSubstitute = Substitute.For<ILogger<WebpageDumperService>>();
        var spinnerLoaderServiceSubstitute = Substitute.For<ISpinnerLoaderService>();
        var progressLoaderServiceSubstitute = Substitute.For<IProgressLoaderService>();
        var sut = new WebpageDumperService(loggerSubstitute, spinnerLoaderServiceSubstitute, progressLoaderServiceSubstitute);

        var uri = new UriBuilder("https://google.com").Uri;
        var command = new DownloadWebpageCommand(uri, 4, "output");
        var htmlFileAsString = $"<!DOCTYPE html><html lang=\"en\" xmlns:fb=\"http://somelink.com\"><head><link rel=\"prefetch\" href=\"/assets/folder1/file1.jpg\" /><script src=\"/assets/js/script1.js\"></script></body></html>";
        var webpageResources = new List<WebpageResource>();
        spinnerLoaderServiceSubstitute.GetWebpageIndex(uri).Returns(htmlFileAsString);
        spinnerLoaderServiceSubstitute.GetWebresourcesLinksFromIndexPage(
            uri,
            htmlFileAsString).Returns(webpageResources);
        progressLoaderServiceSubstitute
            .When(x => x.DownloadWebpageResources(command, webpageResources))
            .Do(x => { return; });

        // Act
        await sut.DumpWebpage(command);

        // Assert
        await spinnerLoaderServiceSubstitute.Received().GetWebpageIndex(uri);
        spinnerLoaderServiceSubstitute.Received().GetWebresourcesLinksFromIndexPage(
            uri,
            htmlFileAsString);
        progressLoaderServiceSubstitute.DidNotReceive().DownloadWebpageResources(
            command,
            webpageResources);
    }
}