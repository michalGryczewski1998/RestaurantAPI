using RestaurantAPI.Model.Models;
using System.Security.Claims;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);
        public IEnumerable<RestaurantDto> GetAll(string? searchPhrase);
        public int Create(CreateRestaurantDto dto);
        public void Delete(int id);
        public void Update(UpdateRestaurantDto resraurant, int id);
    }
}
