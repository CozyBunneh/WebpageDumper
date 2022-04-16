using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NUnit.Framework;
using WebpageDumper.Infrastructure.Webpage.Services;

namespace WebpageDumper.Infrastructure.Webpage.UnitTest;

[TestFixture]
public class WebpageFileParserServiceTest
{
    private Fixture? _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Test]
    public void When_Index_Page_Parsed_With_15_Link_Successfully()
    {
        // Arrange
        var sut = _fixture.Create<WebpageParserService>();
        var scriptId = Guid.NewGuid().ToString();
        var pageUri = new UriBuilder("https://www.site.com").Uri;
        var htmlFileAsString = $"<!DOCTYPE html><html lang=\"en\" xmlns:fb=\"http://somelink.com\"><head><link rel=\"prefetch\" href=\"/assets/folder1/file1.jpg\" /><link rel=\"prefetch\" href=\"/assets/folder1/file2.jpg\" /><link rel=\"prefetch\" href=\"/assets/folder-2/file-3.jpg\" /><meta property=\"og:image\" content=\"https://www.site.com/files/images/file4.png\"/><link rel=\"author\" type=\"text/x-markdown\" href=\"/file5.txt\" /><link rel=\"icon\" type=\"image/png\" href=\"assets/i/favicon.png\" /></head><body><section><video autoplay loop muted playsinline preload=\"metadata\" data-videosrc=\"video1\" src=\"/assets/video/video1.mp4\"></video></section><svg xmlns=\"http://www.w3.org/2000/svg\"><a href=\"https://something.com/some1\">Link1</a><a href=\"/internal/link2\">Link2</a><script src=\"assets/js/lib/polyfills.js\"></script><script src=\"assets/js/lib/common.js?{scriptId}\"></script><script src=\"/assets/js/script1.js\"></script></body></html>";

        // Act
        var files = sut.ParseWebpageForResources(pageUri, htmlFileAsString);

        // Assert
        files.Should().NotBeNull();
        files.Should().NotBeEmpty();
        files.Count.Should().Be(10);
        files[0].fileName.Should().Be("file1.jpg");
        files[0].path.Should().Be("assets/folder1");
        files[1].fileName.Should().Be("file2.jpg");
        files[1].path.Should().Be("assets/folder1");
        files[2].fileName.Should().Be("file-3.jpg");
        files[2].path.Should().Be("assets/folder-2");
        files[3].fileName.Should().Be("file5.txt");
        files[3].path.Should().Be("");
        files[4].fileName.Should().Be("favicon.png");
        files[4].path.Should().Be("assets/i");
        files[5].fileName.Should().Be("video1.mp4");
        files[5].path.Should().Be("assets/video");
        files[6].fileName.Should().Be("polyfills.js");
        files[6].path.Should().Be("assets/js/lib");
        files[7].fileName.Should().Be($"common.js?{scriptId}");
        files[7].path.Should().Be($"assets/js/lib");
        files[8].fileName.Should().Be("script1.js");
        files[8].path.Should().Be("assets/js");
        files[9].fileName.Should().Be("file4.png");
        files[9].path.Should().Be("files/images");
    }
}