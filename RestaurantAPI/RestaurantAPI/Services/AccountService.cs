using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _dbContext;

        public AccountService(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
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
                PassworldHash = string.Empty, //dto.Password,
                PhoneNumber = dto.PhoneNumber,
                RoleId = dto.RoleId
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }
    }
}
