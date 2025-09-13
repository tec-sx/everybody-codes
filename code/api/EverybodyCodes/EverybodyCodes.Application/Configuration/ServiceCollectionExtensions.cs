using EverybodyCodes.Application.Contracts;
using EverybodyCodes.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EverybodyCodes.Application.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<ICameraParser, CameraParser>();
        services.AddHostedService<UpdateDatabaseBackgroundService>();

        return services;
    }
}
