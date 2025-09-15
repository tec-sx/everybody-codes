using EverybodyCodes.Application.Contracts;
using EverybodyCodes.Application.Services;
using FluentAssertions;
using NSubstitute;
using System.IO.Abstractions;

namespace EverybodyCodes.Application.Tets.Services;

public class CameraParserTests
{
    private readonly ICameraParser _sut;
    private readonly IFileSystem _fileSystem;

    public CameraParserTests()
    {
        _fileSystem = Substitute.For<IFileSystem>();
        _sut = new CameraParser(_fileSystem);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_PathNullOrEmpty_ThrowsArgumentException(string? testPath)
    {
        // Arrange
        char separator = ';';

        // Act & Assert
        var result = () => _sut.Parse(testPath, separator);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Parse_FileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange
        string testPath = "nonexistentfile.csv";
        char separator = ';';
        
        _fileSystem.File.Exists(testPath).Returns(false);

        // Act & Assert
        var result = () => _sut.Parse(testPath, separator);
        result.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void Parse_ValidData_ReturnsCameras()
    {
        // Arrange
        string testPath = "test-cameras.csv";
        char separator = ';';
        
        var testData = new string[]
        {
            "Camera;Latitude;Longitude",
            "Camera 1;52.5200;13.4050",
            "Camera 2;48.8566;2.3522",
            "Camera 3;51.5074;-0.1278"
        };

        _fileSystem.File.Exists(testPath).Returns(true);
        _fileSystem.File.ReadAllLines(testPath).Returns(testData);

        // Act
        var result = _sut.Parse(testPath, separator);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
        result[0].Number.Equals(1);
        result[0].Name.Equals("Camera 1");
        result[0].Latitude.Equals(52.5200);
        result[0].Longitude.Equals(13.4050);
    }
}
