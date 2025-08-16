using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            var dishId = _dishService.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{restaurantId}", null);
        }
        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            DishDto dish = _dishService.GetById(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpGet]
        public ActionResult<List<DishDto>> Get([FromRoute] int restaurantId)
        {
            List<DishDto> dishes = _dishService.GetAll(restaurantId);

            return Ok(dishes);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int restaurantID)
        {
            _dishService.RemoveAll(restaurantID);

            return NoContent();
        }
    }
}
