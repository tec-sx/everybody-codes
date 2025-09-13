using EverybodyCodes.Application.Contracts;
using EverybodyCodes.Application.Models;
using EverybodyCodes.Infrastructure.Data;
using EverybodyCodes.Infrastructure.Data.Entities;
using EverybodyCodes.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EverybodyCodes.Application.Services;

internal class UpdateDatabaseBackgroundService : BackgroundService
{
    // Since BackgroundService is registered as a singleton, we need to use IServiceProvider to create scopes for scoped services 
    // instead of injecting them directly.
    private readonly IServiceProvider _serviceProvider;
    
    private readonly ILogger<UpdateDatabaseBackgroundService> _logger;
    private readonly TimeSpan _importInterval = TimeSpan.FromHours(24);
    private const string _dataFileName = "cameras-defb.csv";

    public UpdateDatabaseBackgroundService(IServiceProvider serviceProvider, ILogger<UpdateDatabaseBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Camera Update Background Service started");

        await InitializeAndUpdateAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(_importInterval, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await UpdateDatabase();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Camera Update service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in camera Update background service");
            }
        }

        _logger.LogInformation("Camera Update Background Service stopped");
    }

    private async Task InitializeAndUpdateAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await ctx.Database.EnsureCreatedAsync();
        _logger.LogInformation("Database initialized");

        // Run initial import
        await UpdateDatabase();
    }

    private async Task UpdateDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var cameraParser = scope.ServiceProvider.GetRequiredService<ICameraParser>();
        var cameraService = scope.ServiceProvider.GetRequiredService<ICameraService>();
        var dataPath = Path.Combine(AppContext.BaseDirectory, "data", _dataFileName);

        try
        {
            _logger.LogInformation("Starting camera import from CSV");

            if (!File.Exists(dataPath))
            {
                _logger.LogWarning("CSV file not found: {FilePath}", dataPath);
                return;
            }

            var cameraData = cameraParser.Parse(dataPath, ';');

            if (cameraData.Count == 0)
            {
                _logger.LogWarning("No camera data found in CSV file");
                return;
            }

            await cameraService.AddCamerasBulkAsync(cameraData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during camera import");
        }
    }
}
