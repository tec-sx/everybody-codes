using EverybodyCodes.Application.Contracts.Helpers;
using EverybodyCodes.Application.Contracts.Services;
using EverybodyCodes.Application.Helpers;
using EverybodyCodes.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace EverybodyCodes.Application.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddSingleton<ICameraParser, CameraParser>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<ITimeProvider, SystemTimeProvider>();
        services.AddHostedService<UpdateDatabaseBackgroundService>();
        services.AddScoped<ICameraService, CameraService>();

        return services;
    }
}
