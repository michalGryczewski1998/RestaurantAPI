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
        private readonly IWalidacjaUzytkownika _walidacja;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, ILogger<AccountService> loger, IWalidacjaUzytkownika walidacja)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _loger = loger;
            _walidacja = walidacja;
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

            var walidacja = _walidacja.WalidacjaUzytkownika(newUser);

            if (walidacja.Any(x => x.Item2))
            {
                foreach (var x in walidacja.Where(x => x.Item2 == true))
                {
                    _loger.LogWarning(x.Item1);
                }

                throw new NotCreateAccountException(string.Join(Environment.NewLine, walidacja.Select(x => x.Item1)));
            }

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

    }
}
