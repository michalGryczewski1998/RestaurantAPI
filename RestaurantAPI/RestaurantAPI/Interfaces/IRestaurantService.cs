using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);
        public IEnumerable<RestaurantDto> GetAll();
        public int Create(CreateRestaurantDto dto);
        public bool Delete(int id);
        public bool Update(UpdateRestaurantDto resraurant, int id);
    }
}
