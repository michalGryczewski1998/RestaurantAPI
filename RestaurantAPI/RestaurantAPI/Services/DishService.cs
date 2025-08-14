using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = CzyRestauracjaIstnieje(restaurantId);

            if (!restaurant)
            {
                throw new NotFoundException($"Nie znaleziono restauracji o ID: {restaurantId}");
            }

            var dishEntity = _mapper.Map<Dish>(dto);
            dishEntity.RestaurantId = restaurantId;

            _context.Dishes.Add(dishEntity);
            _context.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = CzyRestauracjaIstnieje(restaurantId);

            if (!restaurant)
            {
                throw new NotFoundException($"Nie znaleziono restauracji o ID: {restaurantId}");
            }

            var dish = _context.Dishes.FirstOrDefault(x => x.Id == dishId);

            if (dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException($"Nie znaleziono dania o ID: {dishId}");
            }

            var dishDto = _mapper.Map<DishDto>(dish);

            return dishDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = _context
                .Restaurants
                .Include(x => x.Dishes)
                .FirstOrDefault(x => x.Id == restaurantId);

            if(restaurant is null)
            {
                throw new NotFoundException($"Nie znaleziono restauracji o ID: {restaurantId}");
            }

            var dishDto = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dishDto;
        }

        private bool CzyRestauracjaIstnieje(int id)
        {
            var restaurant = _context.Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null)
            {
                return false;
            }

            return true;
        }
    }
}
