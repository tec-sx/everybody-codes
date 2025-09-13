using EverybodyCodes.Application.Models;

namespace EverybodyCodes.Application.Contracts;

public interface ICameraService
{
    Task<IReadOnlyCollection<CameraDto>> GetAllCamerasAsync();

    Task<IReadOnlyCollection<CameraDto>> SearchByNameAsync(string name);
    
    Task<CameraDto?> GetCameraByNumberAsync(int number);
    
}
