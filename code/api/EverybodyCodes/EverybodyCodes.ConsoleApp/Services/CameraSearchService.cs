using EverybodyCodes.Application.Contracts;
using EverybodyCodes.ConsoleApp.Contracts;
using EverybodyCodes.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace EverybodyCodes.ConsoleApp.Services;

internal class CameraSearchService : ICameraSearchService
{
    private readonly ICameraService _cameraService;
    private readonly ILogger<CameraSearchService> _logger;

    public CameraSearchService(ICameraService cameraService, ILogger<CameraSearchService> logger)
    {
        _cameraService = cameraService;
        _logger = logger;
    }

    public async Task SearchCamerasAsync(string searchTerm)
    {
        try
        {
            var cameras = await _cameraService.SearchByNameAsync(searchTerm);

            if (cameras.Count == 0)
            {
                Console.WriteLine($"\nNo cameras found containing '{searchTerm}' in the name.");

                return;
            }

            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------------------------");

            foreach (var camera in cameras)
            {
                Console.Write($"{camera.Number} | ");
                Console.Write($"{camera.Name} | ");
                Console.Write($"{camera.Latitude:F6} | ");
                Console.Write($"{camera.Longitude:F6}");
                Console.WriteLine();
            }

            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching cameras");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
