using EverybodyCodes.ConsoleApp.Contracts;
using EverybodyCodes.Infrastructure.Data.Entities;
using EverybodyCodes.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace EverybodyCodes.ConsoleApp.Services;

internal class CameraSearchService : ICameraSearchService
{
    private readonly ICameraRepository _cameraRepository;
    private readonly ILogger<CameraSearchService> _logger;

    public CameraSearchService(ICameraRepository cameraRepository, ILogger<CameraSearchService> logger)
    {
        _cameraRepository = cameraRepository;
        _logger = logger;
    }

    public async Task SearchCamerasAsync(string searchTerm)
    {
        _logger.LogInformation("Searching for cameras containing: '{SearchTerm}'", searchTerm);

        try
        {
            var cameras = await _cameraRepository.SearchByNameAsync(searchTerm);

            if (cameras.Count == 0)
            {
                Console.WriteLine($"\nNo cameras found containing '{searchTerm}' in the name.");

                return;
            }

            foreach (var camera in cameras)
            {
                Console.WriteLine($"{camera.Number} |");
                Console.Write($"{camera.Name} |");
                Console.Write($"{camera.Latitude:F6} |");
                Console.Write($"{camera.Longitude:F6} |");
            }

            _logger.LogInformation("Search completed. Found {Count} matching cameras", cameras.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching cameras");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
