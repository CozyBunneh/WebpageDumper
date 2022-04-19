using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using WebpageDumper.Infrastructure.Webpage.Extensions;

namespace WebpageDumper.Infrastructure.Webpage.UnitTest.Extensions;

public class StringExtensionsTest
{
    [Test]
    [Category("SanitizeHttpResourceString")]
    public void When_Sanitization_Of_HttpResourceString_Is_Success()
    {
        // Arrange
        var httpResourceString = " src=\"/dir/subdir/test.txt\"";
        var leadingStringsToRemove = new List<String>() { "src=" };

        // Act
        var sanitizedHttpResourceString = httpResourceString.SanitizeHttpResourceString(leadingStringsToRemove);

        // Asset
        sanitizedHttpResourceString.Should().Be("dir/subdir/test.txt");
    }

    [Test]
    [Category("IsFile")]
    public void When_IsFile_Is_False()
    {
        // Arrange
        var filenameWithPath = "dir/subdir/test";

        // Act
        var isFile = filenameWithPath.IsFile();

        // Asset
        isFile.Should().Be(false);
    }

    [Test]
    [Category("IsFile")]
    public void When_IsFile_Is_True()
    {
        // Arrange
        var filenameWithPath = "dir/subdir/test.txt";

        // Act
        var isFile = filenameWithPath.IsFile();

        // Asset
        isFile.Should().Be(true);
    }

    [Test]
    [Category("RemoveQuotationMarks")]
    public void When_Removing_QuotationMarks_Is_Successfull()
    {
        // Arrange
        var filenameWithPath = "\"test.txt\"";

        // Act
        var filename = filenameWithPath.RemoveQuotationMarks();

        // Asset
        filename.Should().Be("test.txt");
    }

    [Test]
    [Category("TrimLeadingFrontSlash")]
    public void When_Trimming_Leading_Frontslash_Is_Successfull()
    {
        // Arrange
        var filenameWithPath = "/test.txt";

        // Act
        var filename = filenameWithPath.TrimLeadingFrontSlash();

        // Asset
        filename.Should().Be("test.txt");
    }

    [Test]
    [Category("GetFileNameOfHttpResourceString")]
    public void When_Getting_Filename_With_Path_Is_Successfull()
    {
        // Arrange
        var filenameWithPath = "dir/test.txt";

        // Act
        var filename = filenameWithPath.GetFileNameOfHttpResourceString();

        // Asset
        filename.Should().Be("test.txt");
    }

    [Test]
    [Category("GetFileNameOfHttpResourceString")]
    public void When_Getting_Filename_Without_Path_Is_Successfull()
    {
        // Arrange
        var filenameWithPath = "test.txt";

        // Act
        var filename = filenameWithPath.GetFileNameOfHttpResourceString();

        // Asset
        filename.Should().Be("test.txt");
    }

    [Test]
    [Category("GetDirectoryPathOfHttpResourceString")]
    public void When_Getting_Directory_Path_Is_Successfull()
    {
        // Arrange
        var filenameWithPath = "dir/subdir/test.txt";

        // Act
        var directoryPath = filenameWithPath.GetDirectoryPathOfHttpResourceString();

        // Asset
        directoryPath.Should().Be("dir/subdir");
    }

    [Test]
    [Category("GetDirectoryPathOfHttpResourceString")]
    public void When_Getting_Directory_Path_Is_Unsuccessfull()
    {
        // Arrange
        var filenameWithPath = "test.txt";

        // Act
        var directoryPath = filenameWithPath.GetDirectoryPathOfHttpResourceString();

        // Asset
        directoryPath.Should().Be("");
    }
}
