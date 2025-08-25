using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string FunkcjaHashujaca(User user, string password);
        string GenerateJwt(LoginDto dto);
    }
}
