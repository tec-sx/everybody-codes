using EverybodyCodes.Application.Configuration;
using EverybodyCodes.ConsoleApp.Contracts;
using EverybodyCodes.ConsoleApp.Services;
using EverybodyCodes.Infrastructure.Configuration;
using EverybodyCodes.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace EverybodyCodes.App
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var root = new RootCommand();
            var inputOption = new Option<string>(name: "--name") {  Required = true };

            root.Add(inputOption);

            root.SetAction(async (inputValue, ct) =>
            {
                var input = inputValue.GetValue(inputOption);
                await RunApplicationAsync(input);
            });

            return await root.Parse(args).InvokeAsync();
        }

        private static async Task RunApplicationAsync(string searchName)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();


            var host = CreateHostBuilder().Build();

            // Start the background service
            await host.StartAsync();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                // Run the search
                var searchService = services.GetRequiredService<ICameraSearchService>();
                await searchService.SearchCamerasAsync(searchName);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while searching cameras");
            }
            finally
            {
                await host.StopAsync();
            }
        }

        private static IHostBuilder CreateHostBuilder() =>
           Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
               {
                   // Add services
                   services.AddInfrastructureLayer(context.Configuration);
                   services.AddApplicationLayer();

                   services.AddScoped<ICameraSearchService, CameraSearchService>();

                   // Add logging
                   services.AddLogging(builder =>
                   {
                       builder.AddConsole();
                       builder.SetMinimumLevel(LogLevel.Information);
                   });
               });
    }
}
