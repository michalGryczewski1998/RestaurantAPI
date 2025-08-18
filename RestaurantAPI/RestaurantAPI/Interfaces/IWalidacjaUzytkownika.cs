using RestaurantAPI.Model.Entities;

namespace RestaurantAPI.Interfaces
{
    public interface IWalidacjaUzytkownika
    {
        List<Tuple<string, bool>> WalidacjaUzytkownika(User user);
    }
}
