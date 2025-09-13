using EverybodyCodes.Infrastructure.Data.Entities;

namespace EverybodyCodes.Infrastructure.Data.Repositories;

public interface ICameraRepository : IRepository<CameraEntity>
{
    Task<List<CameraEntity>> SearchByNameAsync(string searchTerm);
    Task<CameraEntity?> GetCameraByNumberAsync(int number);
}
