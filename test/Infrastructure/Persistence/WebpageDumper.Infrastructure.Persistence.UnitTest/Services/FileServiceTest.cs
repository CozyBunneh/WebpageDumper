using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NSubstitute;
using NUnit.Framework;
using WebpageDumper.Infrastructure.Persistence.Services;

namespace WebpageDumper.Infrastructure.Persistence.UnitTest.Services;

/// <summary>
///     Not many unit tests here since mocking of static classes isn't possible,
///     we wouldn't want tests to start creating directories in the filesystem.
///     Nor should private methods be tested directly.
/// </summary>
[TestFixture]
public class FileServiceTest
{
    private Fixture? _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Test]
    public async Task WriteFileTestAsync_Successful()
    {
        // Arrange
        var sut = _fixture.Create<FileService>();
        var fileStreamSubstitute = Substitute.For<Stream>();
        fileStreamSubstitute.CopyToAsync(Arg.Any<FileStream>()).Returns(Task.CompletedTask);
        var fileName = "test.txt";

        // Act
        await sut.WriteFile(fileStreamSubstitute, fileName);

        // Assert
        await fileStreamSubstitute.Received().CopyToAsync(Arg.Any<FileStream>());
    }
}
