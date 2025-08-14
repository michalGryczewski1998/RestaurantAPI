using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
    }
}
