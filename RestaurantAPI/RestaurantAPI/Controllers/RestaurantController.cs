using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Information;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Xml;
using System.Xml.Serialization;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
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

        [HttpPut("update/{Id}")]
        public ActionResult Update([FromBody] UpdateRestaurantDto resraurant, [FromRoute] int Id)
        {
            _restaurantService.Update(resraurant, Id);

             return Ok();
        }

        [HttpDelete("{Id}")]
        public ActionResult Delete([FromRoute] int Id)
        {
            _restaurantService.Delete(Id);

            return NotFound($"Usunięto restaurację o {Id}");
        }

        [HttpPost("")]
        [Authorize(Roles = "Admin, Manager, User")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto) 
        {
            var userId = int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var resId = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{resId}", null);
        }

        [HttpGet]
        [Authorize(Policy = "Atleast20")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurantsDto = _restaurantService.GetAll();

            return Ok(restaurantsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);

            return Ok(restaurant);
        }       
    }
}
