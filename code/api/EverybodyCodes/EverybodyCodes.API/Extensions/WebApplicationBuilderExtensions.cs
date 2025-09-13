using EverybodyCodes.API.Infrastructure;
using EverybodyCodes.Application.Configuration;
using EverybodyCodes.Infrastructure.Configuration;

namespace EverybodyCodes.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        return builder;
    }

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructureLayer(builder.Configuration);
        builder.Services.AddApplicationLayer();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public static WebApplicationBuilder ConfigureCORS(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            // Define a CORS policy for local development.
            if (builder.Environment.IsDevelopment())
            {
                options.AddPolicy("localClientApp", policyBuilder =>
                {
                    policyBuilder.WithOrigins("http://127.0.0.1:5500");
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                    policyBuilder.AllowCredentials();
                });
            }
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureExceptionHandling(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        return builder;
    }
}
