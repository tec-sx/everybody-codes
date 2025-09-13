using EverybodyCodes.Infrastructure.Data.Entities;
using EverybodyCodes.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EverybodyCodes.Infrastructure.Data.Repositoriesl;

public class CameraRepository : ICameraRepository
{
    private readonly ApplicationDbContext _ctx;
    public CameraRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<CameraEntity> AddAsync(CameraEntity entity)
    {
        var camera = await _ctx.Cameras.AddAsync(entity);
        await _ctx.SaveChangesAsync();

        return camera.Entity;
    }

    public Task<List<CameraEntity>> GetAllAsync()
    {
        return _ctx.Cameras
           .Where(r => r.DeletedAt == null)
           .ToListAsync();
    }

    public Task<CameraEntity> GetByIdAsync(int id)
    {
        return _ctx.Cameras
            .FirstAsync(r => r.Id == id && r.DeletedAt == null);
    }

    public Task<List<CameraEntity>> SearchByNameAsync(string searchTerm)
    {
        return _ctx.Cameras
            .Where(c => c.Name.Contains(searchTerm))
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
