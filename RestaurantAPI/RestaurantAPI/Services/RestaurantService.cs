using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public bool Delete(int id)
        {
            _logger.LogWarning($"Restauracja z ID {id} wywołanie metody DELETE");
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null) return false;

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

            _logger.LogWarning($"Restauracja z ID {id} została usunięta");

            return true;

        }

        public RestaurantDto GetById(int id)
        {
            _logger.LogWarning($"Restauracja z ID {id} wywołanie metody GetById");
            var restaurant = GetRestaurantById(id);

            if (restaurant == null) return null;

            _logger.LogWarning($"Restauracja z ID {id} została pobrana poprawnie");

            var res = _mapper.Map<RestaurantDto>(restaurant);
            return res;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            _logger.LogWarning($"Wywołanie metody GetAll");

            var restaurants = GetAllRestaurants();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            _logger.LogWarning($"Wywołanie metody GetAll zakończone pomyślnie");

            return restaurantsDto;
        }

        public int Create(CreateRestaurantDto dto)
        {
            _logger.LogWarning($"Wywołanie metody Create");

            var restaurant = _mapper.Map<Restaurant>(dto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            _logger.LogWarning($"Wywołanie metody Create zakończone pomyślnie");

            return restaurant.Id;
        }

        public bool Update(UpdateRestaurantDto resraurantRequest, int id)
        {
            _logger.LogWarning($"Wywołanie metody Update dla restauracji {id}");

            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null) return false;

            restaurant.UpdateFrom(resraurantRequest);
            _dbContext .SaveChanges();

            _logger.LogWarning($"Wywołanie metody Update dla restauracji {id} zakończone pomyślnie");

            return true;
        }

        private Restaurant? GetRestaurantById(int Id)
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(x => x.Adress)
                .Include(x => x.Dishes)
                .FirstOrDefault(x => x.Id == Id);

            if (restaurants == null)
            {
                return null;
                _logger.LogWarning($"Nie znaleziono restauracji o {Id}");
            }

            return restaurants;
        }

        private List<Restaurant>? GetAllRestaurants()
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(x => x.Adress)
                .Include(x => x.Dishes)
                .ToList();

            if (restaurants == null)
            {
                return null;
                _logger.LogWarning($"Nie pobrano restauracji");
            }

            return restaurants;
        }
    }
}
