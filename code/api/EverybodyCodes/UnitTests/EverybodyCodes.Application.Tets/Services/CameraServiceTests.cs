using EverybodyCodes.Application.Contracts.Services;
using EverybodyCodes.Application.Models;
using EverybodyCodes.Application.Services;
using EverybodyCodes.Infrastructure.Data.Entities;
using EverybodyCodes.Infrastructure.Data.Repositories;
using FluentAssertions;
using NSubstitute;

namespace EverybodyCodes.Application.Tets.Services;

public class CameraServiceTests
{
    private readonly ICameraRepository _cameraRepository;
    private readonly ICameraService _sut;

    public CameraServiceTests()
    {
        _cameraRepository = Substitute.For<ICameraRepository>();
        _sut = new CameraService(_cameraRepository);
    }

    [Fact]
    public async Task AddCameraAsync_ValidCameraDto_CallsRepositoryAddAndSaveChanges()
    {
        // Arrange
        var cameraDto = new CameraDto
        {
            Number = 1,
            Name = "Test Camera",
            Latitude = 52.5200f,
            Longitude = 13.4050f
        };
        // Act
        await _sut.AddCameraAsync(cameraDto);

        // Assert
        await _cameraRepository.Received(1).AddAsync(Arg.Is<CameraEntity>(c =>
            c.Number == cameraDto.Number &&
            c.Name == cameraDto.Name &&
            c.Latitude == cameraDto.Latitude &&
            c.Longitude == cameraDto.Longitude));

        await _cameraRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task AddCamerasBulkAsync_EmptyList_DoesNotCallRepository()
    {   // Arrange
        var emptyCameraList = new List<CameraDto>();

        // Act
        Func<Task> act = async () => await _sut.AddCamerasBulkAsync(emptyCameraList);

        // Assert
        await act.Should().NotThrowAsync();
        await _cameraRepository.DidNotReceive().BulkUpsertCameras(Arg.Any<IEnumerable<CameraEntity>>());
    }

    [Fact]
    public async Task AddCamerasBulkAsync_ValidCameraDtos_CallsRepositoryBulkUpsert()
    {
        // Arrange
        var cameraDtos = new List<CameraDto>
        {
            new CameraDto { Number = 1, Name = "Camera 1", Latitude = 52.5200f, Longitude = 13.4050f },
            new CameraDto { Number = 2, Name = "Camera 2", Latitude = 48.8566f, Longitude = 2.3522f }
        };

        // Act
        await _sut.AddCamerasBulkAsync(cameraDtos);

        // Assert
        await _cameraRepository.Received(1).BulkUpsertCameras(Arg.Is<IEnumerable<CameraEntity>>(entities =>
            entities.Count() == cameraDtos.Count &&
            entities.Any(e => e.Number == 1 && e.Name == "Camera 1" && e.Latitude == 52.5200f && e.Longitude == 13.4050f) &&
            entities.Any(e => e.Number == 2 && e.Name == "Camera 2" && e.Latitude == 48.8566f && e.Longitude == 2.3522f)
        ));
    }

    [Fact]
    public async Task GetAllCamerasAsync_ReturnsMappedCameraDtos()
    {
        // Arrange
        var cameraEntities = new List<CameraEntity>
        {
            new CameraEntity { Number = 1, Name = "Camera 1", Latitude = 52.5200f, Longitude = 13.4050f },
            new CameraEntity { Number = 2, Name = "Camera 2", Latitude = 48.8566f, Longitude = 2.3522f }
        };
        _cameraRepository.GetAllAsync().Returns(Task.FromResult(cameraEntities));
        
        // Act
        var result = await _sut.GetAllCamerasAsync();
        
        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Number == 1 && c.Name == "Camera 1" && c.Latitude == 52.5200f && c.Longitude == 13.4050f);
        result.Should().Contain(c => c.Number == 2 && c.Name == "Camera 2" && c.Latitude == 48.8566f && c.Longitude == 2.3522f);
    }
}
