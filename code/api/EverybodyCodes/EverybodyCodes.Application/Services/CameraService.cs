using EverybodyCodes.Application.Contracts;
using EverybodyCodes.Application.Models;
using EverybodyCodes.Infrastructure.Data.Repositories;

namespace EverybodyCodes.Application.Services;

public class CameraService : ICameraService
{
    private readonly ICameraRepository _cameraRepository;

    public CameraService(ICameraRepository cameraRepository)
    {
        _cameraRepository = cameraRepository;
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

    public async Task<CameraDto?> GetCameraByNumberAsync(int number)
    {
        var cameraEntity = await _cameraRepository.GetCameraByNumberAsync(number);

        return new CameraDto
        {
            Number = cameraEntity?.Number ?? 0,
            Name = cameraEntity?.Name ?? string.Empty,
            Latitude = cameraEntity?.Latitude ?? 0,
            Longitude = cameraEntity?.Longitude ?? 0
        };
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
