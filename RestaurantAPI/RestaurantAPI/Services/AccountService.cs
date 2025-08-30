using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Authentication;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger _loger;
        private readonly IWalidacjaUzytkownika _walidacja;
        private readonly AuthenticationSettings _settings;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, ILogger<AccountService> loger, IWalidacjaUzytkownika walidacja, AuthenticationSettings settings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _loger = loger;
            _walidacja = walidacja;
            _settings = settings;
        }

        public string FunkcjaHashujaca(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Email == dto.Email) ?? throw new BadRequestException("Niepoprawny adres e-mail lub hasło");
            var res = _passwordHasher.VerifyHashedPassword(user, user.PassworldHash, dto.Password);

            if(res == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Niepoprawny adres e-mail lub hasło");
            }

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Role, $"{user.Role.Name}"),
                new("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
            };

            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add
                    (
                        new Claim("Nationality", user.Nationality)
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtKey));
            // Przekazaliśmy klucz prywatny i wybraliśmy algorytm hashowania SHA256
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Twaorzymy logikę która określi do jakiej daty ten token będzie aktywny
            var expires = DateTime.Now.AddDays(_settings.JwtExpireDays);
            // Tworzymy token
            var token = new JwtSecurityToken(_settings.JwtIssuer,
                _settings.JwtIssuer, 
                claims, 
                expires: expires, 
                signingCredentials: cred);

            // zapisujemy token jako string za pomocą JwtSecurityTokenHandler
            var tokenhandler = new JwtSecurityTokenHandler();
            return tokenhandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User
            {
                FirstName = dto.FirstName,
                LastName = string.Empty,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                PhoneNumber = dto.PhoneNumber,
                RoleId = dto.RoleId
            };

            newUser.PassworldHash = FunkcjaHashujaca(newUser, dto.Password);

            var walidacja = _walidacja.WalidacjaUzytkownika(newUser);

            if (walidacja.Any(x => x.Item2))
            {
                foreach (var x in walidacja.Where(x => x.Item2 == true))
                {
                    _loger.LogWarning(message: x.Item1);
                }

                throw new NotCreateAccountException(string.Join(Environment.NewLine, walidacja.Select(x => x.Item1)));
            }

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

    }
}
