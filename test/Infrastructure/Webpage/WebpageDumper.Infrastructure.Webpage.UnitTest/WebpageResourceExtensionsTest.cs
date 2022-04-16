using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using WebpageDumper.Infrastructure.Webpage.Extensions;

namespace WebpageDumper.Infrastructure.Webpage.UnitTest;

public class WebpageResourceExtensionsTest
{
    [Test]
    [Category("ToDtos")]
    public void When_ToDtos_Is_Success()
    {
        // Arrange
        var httpResourceStrings = new List<String>() { "dir/subdir/test.txt", "dir2/subdir2/test2.txt" };

        // Act
        var webpageResources = httpResourceStrings.ToDtos();

        // Asset
        webpageResources.Should().NotBeNull();
        webpageResources.Count.Should().Be(2);
        webpageResources[0].fileName.Should().NotBeNull();
        webpageResources[0].fileName.Should().Be("test.txt");
        webpageResources[0].path.Should().NotBeNull();
        webpageResources[0].path.Should().Be("dir/subdir");
        webpageResources[1].fileName.Should().NotBeNull();
        webpageResources[1].fileName.Should().Be("test2.txt");
        webpageResources[1].path.Should().NotBeNull();
        webpageResources[1].path.Should().Be("dir2/subdir2");
    }

    [Test]
    [Category("ToDto")]
    public void When_ToDto_Is_Success()
    {
        // Arrange
        var httpResourceString = "dir/subdir/test.txt";

        // Act
        var webpageResource = httpResourceString.ToDto();

        // Asset
        webpageResource.Should().NotBeNull();
        webpageResource.fileName.Should().NotBeNull();
        webpageResource.fileName.Should().Be("test.txt");
        webpageResource.path.Should().NotBeNull();
        webpageResource.path.Should().Be("dir/subdir");
    }
}