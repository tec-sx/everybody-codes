using EverybodyCodes.Infrastructure.Data;
using EverybodyCodes.Infrastructure.Data.Entities;
using EverybodyCodes.Infrastructure.Data.Repositories;
using EverybodyCodes.Infrastructure.Data.Repositoriesl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EverybodyCodes.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Scoped);

        // Add repositories
        services.AddScoped<ICameraRepository, CameraRepository>();

        return services;
    }
}
