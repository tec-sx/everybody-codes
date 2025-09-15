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
            .Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _ctx.SaveChangesAsync();
    }

    public async Task BulkUpsertCameras(IEnumerable<CameraEntity> cameraEntities)
    {
        if (!cameraEntities.Any())
        {
            return;
        }

        var cameraNames = cameraEntities.Select(c => c.Name).ToList();

        var existingCameras = await _ctx.Cameras
            .AsNoTracking()
            .Where(c => cameraNames.Contains(c.Name))
            .ToDictionaryAsync(c => c.Name);

        foreach (var cameraEntity in cameraEntities)
        {
            if (existingCameras.TryGetValue(cameraEntity.Name, out var existingCamera))
            {
                var cameraToUpdate = existingCamera with { Latitude = cameraEntity.Latitude, Longitude = cameraEntity.Longitude };
                _ctx.Cameras.Update(cameraToUpdate);
            }
            else
            {
                await _ctx.Cameras.AddAsync(cameraEntity);
            }
        }

        await _ctx.SaveChangesAsync();
    }
}
