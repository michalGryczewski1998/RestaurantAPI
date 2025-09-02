using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Enums;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContext;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService, IUserContextService userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContext = userContext;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Restauracja z ID {id} wywołanie metody DELETE");
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null) throw new NotFoundException($"Nie znaleziono zasobu o ID: {id}");

            var autherizationRes = _authorizationService.AuthorizeAsync(_userContext.User, restaurant,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!autherizationRes.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

            _logger.LogWarning($"Restauracja z ID {id} została usunięta");
        }

        public RestaurantDto GetById(int id)
        {
            _logger.LogWarning($"Restauracja z ID {id} wywołanie metody GetById");
            var restaurant = GetRestaurantById(id);

            if (restaurant == null) throw new NotFoundException($"Nie znaleziono zasobu o ID: {id}");

            _logger.LogWarning($"Restauracja z ID {id} została pobrana poprawnie");

            var res = _mapper.Map<RestaurantDto>(restaurant);
            return res;
        }

        public PageResult<RestaurantDto> GetAll(RestaurantQuery query)
        {
            _logger.LogWarning($"Wywołanie metody GetAll");

            var restaurants = GetAllRestaurants(query, out int totalCount);

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
            var pageResult = new PageResult<RestaurantDto>(restaurantsDto, totalCount, query.PageSize, query.PageSize);
            _logger.LogWarning($"Wywołanie metody GetAll zakończone pomyślnie");

            return pageResult;
        }

        public int Create(CreateRestaurantDto dto)
        {
            _logger.LogWarning($"Wywołanie metody Create");

            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContext.GetUserId;
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            _logger.LogWarning($"Wywołanie metody Create zakończone pomyślnie");

            return restaurant.Id;
        }

        public void Update(UpdateRestaurantDto resraurantRequest, int id)
        {
            _logger.LogWarning($"Wywołanie metody Update dla restauracji {id}");

            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null) throw new NotFoundException($"Nie znaleziono zasobu o ID: {id}");

            var autherizationRes = _authorizationService.AuthorizeAsync(_userContext.User, restaurant, 
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!autherizationRes.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.UpdateFrom(resraurantRequest);
            _dbContext .SaveChanges();

            _logger.LogWarning($"Wywołanie metody Update dla restauracji {id} zakończone pomyślnie");
        }

        private Restaurant? GetRestaurantById(int id)
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(x => x.Adress)
                .Include(x => x.Dishes)
                .FirstOrDefault(x => x.Id == id);

            if (restaurants == null)
            {
                throw new NotFoundException($"Nie znaleziono zasobu o ID: {id}");
            }

            return restaurants;
        }

        private List<Restaurant>? GetAllRestaurants(RestaurantQuery query, out int totalCount)
        {
            var baseQuery = _dbContext
                .Restaurants
                .Include(x => x.Adress)
                .Include(x => x.Dishes)
                // IsNullOrEmpty(searchPhrase) sprawi, że jeśli searchPhrase jest null lub puste to
                // warunek zawsze true, więc zwróci mi i tak wszystkie rekordy
                .Where(x => string.IsNullOrEmpty(query.SearchPhrase) ||
                    x.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                    x.Description.ToLower().Contains(query.SearchPhrase.ToLower()));

            var restaurants = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            totalCount = baseQuery.Count();

            if (restaurants == null)
            {
                throw new NotFoundException($"Nie znaleziono zasobu");
            }

            return restaurants;
        }
    }
}
