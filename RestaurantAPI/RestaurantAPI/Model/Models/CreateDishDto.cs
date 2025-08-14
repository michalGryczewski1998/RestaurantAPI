using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Model.Models
{
    public class CreateDishDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
    }
}