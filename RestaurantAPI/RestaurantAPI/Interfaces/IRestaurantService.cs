using RestaurantAPI.Model.Models;
using System.Security.Claims;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);
        public IEnumerable<RestaurantDto> GetAll();
        public int Create(CreateRestaurantDto dto, int userId);
        public void Delete(int id, ClaimsPrincipal user);
        public void Update(UpdateRestaurantDto resraurant, int id, ClaimsPrincipal user);
    }
}
