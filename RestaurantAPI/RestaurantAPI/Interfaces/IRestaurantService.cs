using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);
        public IEnumerable<RestaurantDto> GetAll();
        public int Create(CreateRestaurantDto dto);
        public void Delete(int id);
        public void Update(UpdateRestaurantDto resraurant, int id);
    }
}
