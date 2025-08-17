using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger _loger;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, ILogger<AccountService> loger)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _loger = loger;
        }

        public string FunkcjaHashujaca(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
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

            var walidacja = WalidacjaUzytkownika(newUser);

            if (walidacja.Any(x => x.Value == true))
            {
                foreach (var x in walidacja.Where(x => x.Value == true))
                {
                    _loger.LogWarning(x.Key);
                }

                throw new NotCreateAccountException(string.Join(Environment.NewLine, walidacja.Keys));
            }

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

        public Dictionary<string, bool> WalidacjaUzytkownika(User user)
        {
            var status = new Dictionary<string, bool>();

            var istniejeUzytkownikNaTenEmail = _dbContext.Users
                .Where(x => x.Email == user.Email);

            var istniejeUzytkownikNaTenNumerTelefonu = _dbContext.Users
                .Where(x => x.PhoneNumber == user.PhoneNumber);

            if (istniejeUzytkownikNaTenEmail.Any())
            {
                status.Add($"Użytkownik {user.FirstName} musi podać inny adres E-Mail, podany adres {user.Email} jest używany !", true);
            }

            if (istniejeUzytkownikNaTenNumerTelefonu.Any())
            {
                status.Add($"Użytkownik {user.FirstName} musi podać inny numer telefonu, podany numer {user.PhoneNumber} jest używany !", true);
            }

            return status;
        }
    }
}
