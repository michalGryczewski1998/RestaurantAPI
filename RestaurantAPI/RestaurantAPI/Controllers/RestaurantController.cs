using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper, IRestaurantService restaurantService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _restaurantService = restaurantService;
        }

        [HttpPost("")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resId = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{resId}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurantsDto = _restaurantService.GetAll();

            return Ok(restaurantsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);

            if (restaurant is null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }
    }
}
