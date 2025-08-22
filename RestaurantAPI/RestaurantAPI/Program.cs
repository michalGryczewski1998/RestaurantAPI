using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using NLog.Web;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Middleware;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;
using RestaurantAPI.Model.Seed;
using RestaurantAPI.Model.Validators;
using RestaurantAPI.Services;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddFluentValidation();
        builder.Services.AddDbContext<RestaurantDbContext>();
        builder.Services.AddScoped<RestaurantSeeder>();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeMiddleware>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
        builder.Services.AddScoped<IValidator<CreateRestaurantDto>, CreateRestaurantValidator>();
        builder.Services.AddScoped<IValidator<CreateDishDto>, CreateDishValidator>();
        builder.Services.AddSwaggerGen();

        builder.UseNLog();

        builder.Services.AddScoped<IRestaurantService, RestaurantService>();
        builder.Services.AddScoped<IDishService, DishService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IWalidacjaUzytkownika, RegisterUserDtoValidator>();
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
        app.UseMiddleware<RequestTimeMiddleware>();

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