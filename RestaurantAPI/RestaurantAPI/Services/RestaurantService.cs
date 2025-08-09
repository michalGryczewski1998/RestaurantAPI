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

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool Delete(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null) return false;

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

            return true;

        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Adress)
                .Include(x => x.Dishes)
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null) return null;

            var res = _mapper.Map<RestaurantDto>(restaurant);
            return res;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(x => x.Adress)
                .Include(x => x.Dishes)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
            return restaurantsDto;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public bool Update(UpdateRestaurantDto resraurantRequest, int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null) return false;

            restaurant.Name = resraurantRequest.Name;
            restaurant.Description = resraurantRequest.Description;
            restaurant.Category = resraurantRequest.Category;
            restaurant.HasDelivery = resraurantRequest.HasDelivery;

            _dbContext.Restaurants.Update(restaurant);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
