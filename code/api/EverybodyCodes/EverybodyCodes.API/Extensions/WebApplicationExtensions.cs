namespace EverybodyCodes.API.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication Configure(this WebApplication app)
    {
        // Configure Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Configure CORS
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("localClientApp");
        }

        app.UseExceptionHandler();
        app.MapControllers();

        return app;
    }
}
