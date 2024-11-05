using Scalar.AspNetCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers();

        var app = builder.Build();

        // Expose OpenAPI docs
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "/openapi/{documentName}.json";
        });
        app.MapScalarApiReference();

        app.UseHttpsRedirection();
        app.UseWebSockets();

        app.UseRouting();
        app.MapControllers();

        await app.RunAsync();
    }
}