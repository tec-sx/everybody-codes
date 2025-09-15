using EverybodyCodes.Application.Configuration;
using EverybodyCodes.ConsoleApp.Contracts;
using EverybodyCodes.ConsoleApp.Services;
using EverybodyCodes.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;

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
            var host = CreateHostBuilder().Build();
            await host.StartAsync();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var searchService = services.GetRequiredService<ICameraSearchService>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                // Run the initial search
                await searchService.SearchCamerasAsync(searchName);
                await RunApplicationLoop(searchService, logger);
            }
            catch (Exception ex)
            {
                
                logger.LogError(ex, "An error occurred while searching cameras");
            }
            finally
            {
                await host.StopAsync();
            }
        }

        private static async Task RunApplicationLoop(ICameraSearchService searchService, ILogger logger)
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("  --name <search_term>    Search for cameras");
            Console.WriteLine("  --help                  Show this help");
            Console.WriteLine("  --exit                  Exit application");
            Console.WriteLine();

            while (true)
            {
                Console.Write("\nEnter command: ");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Please enter a command. Type --help for available commands.");
                    continue;
                }

                var args = ParseInput(input);

                if (args.Length == 0)
                {
                    Console.WriteLine("Please enter a valid command. Type --help for available commands.");
                    continue;
                }

                var command = args[0].ToLower();

                try
                {
                    switch (command)
                    {
                        case "--name":
                            if (args.Length < 2 || string.IsNullOrWhiteSpace(args[1]))
                            {
                                Console.WriteLine("Error: --name requires a search term.");
                                Console.WriteLine("Usage: --name <search_term>");
                                break;
                            }

                            await searchService.SearchCamerasAsync(args[1]);
                            break;

                        case "--help":
                            Console.WriteLine("Commands:");
                            Console.WriteLine("  --name <search_term>    Search for cameras");
                            Console.WriteLine("  --help                  Show this help");
                            Console.WriteLine("  --exit                  Exit application");
                            Console.WriteLine();
                            break;

                        case "--exit":
                            Console.WriteLine("Exiting...");
                            return;

                        default:
                            Console.WriteLine($"Unknown command: {command}");
                            Console.WriteLine("Type --help to see available commands.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error executing command: {Command}", command);
                }
            }

        }

        private static string[] ParseInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Array.Empty<string>();
            }

            var parts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                return new[] { parts[0] };
            }

            return new[] { parts[0], parts[1] };
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
