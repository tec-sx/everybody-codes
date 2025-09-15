using EverybodyCodes.Application.Models;

namespace EverybodyCodes.Application.Contracts;

public interface ICameraService
{
    Task<IReadOnlyCollection<CameraDto>> GetAllCamerasAsync();

    Task<IReadOnlyCollection<CameraDto>> SearchByNameAsync(string name);
    
    Task AddCameraAsync(CameraDto cameraDto);

    Task AddCamerasBulkAsync(IEnumerable<CameraDto> cameraDtos);
}
