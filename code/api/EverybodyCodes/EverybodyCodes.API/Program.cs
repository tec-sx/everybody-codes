
using EverybodyCodes.API.Extensions;

namespace EverybodyCodes.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.ConfigureLogging()
                .ConfigureServices()
                .ConfigureCORS()
                .ConfigureExceptionHandling();

            var app = builder.Build();

            app.Configure();

            app.Run();
        }
    }
}
