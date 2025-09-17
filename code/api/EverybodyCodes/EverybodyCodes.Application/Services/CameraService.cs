using EverybodyCodes.Application.Contracts.Services;
using EverybodyCodes.Application.Models;
using EverybodyCodes.Infrastructure.Data.Entities;
using EverybodyCodes.Infrastructure.Data.Repositories;

namespace EverybodyCodes.Application.Services;

public class CameraService : ICameraService
{
    private readonly ICameraRepository _cameraRepository;

    public CameraService(ICameraRepository cameraRepository)
    {
        _cameraRepository = cameraRepository;
    }

    public async Task AddCameraAsync(CameraDto cameraDto)
    {
        var cameraEntity = new CameraEntity
        {
            Number = cameraDto.Number,
            Name = cameraDto.Name,
            Latitude = cameraDto.Latitude,
            Longitude = cameraDto.Longitude
        };

        await _cameraRepository.AddAsync(cameraEntity);
        await _cameraRepository.SaveChangesAsync();
    }

    public async Task AddCamerasBulkAsync(IEnumerable<CameraDto> cameraDtos)
    {
        if (!cameraDtos.Any())
        {
            return;
        }

        var camerasEntities = cameraDtos.Select(cameraDtos => new CameraEntity
        {
            Number = cameraDtos.Number,
            Name = cameraDtos.Name,
            Latitude = cameraDtos.Latitude,
            Longitude = cameraDtos.Longitude
        });
        
        await _cameraRepository.BulkUpsertCameras(camerasEntities);
    }

    public async Task<IReadOnlyCollection<CameraDto>> GetAllCamerasAsync()
    {
        var cameraEntities = await _cameraRepository.GetAllAsync();
        
        return cameraEntities.Select(cameraEntity => new CameraDto
        {
            Number = cameraEntity?.Number ?? 0,
            Name = cameraEntity?.Name ?? string.Empty,
            Latitude = cameraEntity?.Latitude ?? 0,
            Longitude = cameraEntity?.Longitude ?? 0
        }).ToList();
    }

    public async Task<IReadOnlyCollection<CameraDto>> SearchByNameAsync(string name)
    {
        var cameraEntities = await _cameraRepository.SearchByNameAsync(name);

        return cameraEntities.Select(cameraEntities => new CameraDto
        {
            Number = cameraEntities.Number,
            Name = cameraEntities.Name,
            Latitude = cameraEntities.Latitude,
            Longitude = cameraEntities.Longitude
        }).ToList();
    }
}
