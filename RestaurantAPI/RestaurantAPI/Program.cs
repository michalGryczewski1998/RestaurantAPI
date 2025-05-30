using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Seed;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddDbContext<RestaurantDbContext>();
        builder.Services.AddScoped<RestaurantSeeder>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        // Tworzymy scope (zakres us�ug) dla r�cznego pobrania serwis�w
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var seeder = services.GetRequiredService<RestaurantSeeder>();
                seeder.Seed(); // tutaj wywo�ujesz Seed()
            }
            catch (Exception ex)
            {
                // logowanie b��d�w, je�li chcesz
                Console.WriteLine($"B��d podczas seedowania danych: {ex.Message}");
            }
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}