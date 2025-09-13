using EverybodyCodes.Application.Contracts;
using EverybodyCodes.Infrastructure.Data;
using EverybodyCodes.Infrastructure.Data.Entities;
using EverybodyCodes.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EverybodyCodes.Application.Services;

internal class UpdateDatabaseBackgroundService : BackgroundService
{
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

        // Ensure database is created
        await ctx.Database.EnsureCreatedAsync();
        _logger.LogInformation("Database initialized");

        // Run initial import
        await UpdateDatabase();
    }

    private async Task UpdateDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var cameraParser = scope.ServiceProvider.GetRequiredService<ICameraParser>();
        var cameraRepository = scope.ServiceProvider.GetRequiredService<ICameraRepository>();
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
            foreach (var dto in cameraData)
            {
                try
                {
                    var camera = new CameraEntity
                    {
                        Number = dto.Number,
                        Name = dto.Name,
                        Latitude = dto.Latitude,
                        Longitude = dto.Longitude
                    };

                    await cameraRepository.AddAsync(camera);

                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error importing camera: {Name}", dto.Name);
                }
            }

            await cameraRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during camera import");
        }
    }
}
