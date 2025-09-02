using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using RestaurantAPI.Authentication;
using RestaurantAPI.Authorization;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Middleware;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;
using RestaurantAPI.Model.Seed;
using RestaurantAPI.Model.Validators;
using RestaurantAPI.Services;
using System.Reflection;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var authenticationSettings = new AuthenticationSettings();
        builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
        builder.Services.AddSingleton(authenticationSettings);

        builder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = "Bearer";
            option.DefaultScheme = "Bearer";
            option.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(conf =>
        {
            conf.RequireHttpsMetadata = false; // nie wymuszamy przez klienta tylko protoko³u https
            conf.SaveToken = true; // token powinien zostaæ zapisany po stronie serwera, do celów autentykacji
            // !!  PARAMETRY WALIDACJI !!
            conf.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                // sprawdzamy czy dany token jest zgodny z tym co wie serwer
                ValidIssuer = authenticationSettings.JwtIssuer, // wydawca tokenu
                ValidAudience = authenticationSettings.JwtIssuer, // jakie podmioty mog¹ u¿ywaæ tego tokenu
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)), // klucz prywatny wygenerowany
                // na podstawie authenticationSettings.JwtKey
            };
        });

        builder.Services.AddAuthorization(option =>
        {
            option.AddPolicy("HasNationality", // Nazwa opcji konfiguracji
                builder => builder.RequireClaim("Nationality", "Polish", "German")); // drugi parametr to warunki
            /* --------------------------------------------------------------------------
             * Sprawdzamy czy User ma jak¹œ narodowoœæ
             * W warunkach wywo³ujemy RequireClaim na builder
             * za jego pomoc¹ po nazwie sprawdzamy czy dany Claim istnieje w tokenie JWT
             * "Polish", "German" to wartoœci dodatkowe jakie ten claim musi mieæ
             * --------------------------------------------------------------------------
             */

            // Walidacja wieku u¿ytkownika
            option.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
        });

        builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
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
        builder.Services.AddScoped<IValidator<UpdateRestaurantDto>, UpdateRestaurantValidator>();
        builder.Services.AddScoped<IValidator<LoginDto>, LoginValidator>();
        builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
        builder.Services.AddSwaggerGen();

        builder.UseNLog();

        builder.Services.AddScoped<IRestaurantService, RestaurantService>();
        builder.Services.AddScoped<IDishService, DishService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IWalidacjaUzytkownika, RegisterUserDtoValidator>();
        builder.Services.AddScoped<IUserContextService, UserContextService>();
        builder.Services.AddHttpContextAccessor();
        var app = builder.Build();

        // Configure the HTTP request pipeline.

        // Tworzymy scope (zakres us³ug) dla rêcznego pobrania serwisów
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var seeder = services.GetRequiredService<RestaurantSeeder>();
                seeder.Seed(); // wywo³uje Seed()
            }
            catch (Exception ex)
            {
                // logowanie b³êdów
                Console.WriteLine($"B³¹d podczas seedowania danych: {ex.Message}");
            }
        }
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<RequestTimeMiddleware>();
        app.UseAuthentication();

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