using NLog.Web;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Middleware;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Seed;
using RestaurantAPI.Services;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddDbContext<RestaurantDbContext>();
        builder.Services.AddScoped<RestaurantSeeder>();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddSwaggerGen();

        builder.UseNLog();

        builder.Services.AddScoped<IRestaurantService, RestaurantService>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.

        // Tworzymy scope (zakres us³ug) dla rêcznego pobrania serwisów
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var seeder = services.GetRequiredService<RestaurantSeeder>();
                seeder.Seed(); // tutaj wywo³ujesz Seed()
            }
            catch (Exception ex)
            {
                // logowanie b³êdów, jeœli chcesz
                Console.WriteLine($"B³¹d podczas seedowania danych: {ex.Message}");
            }
        }
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI");
        });

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}