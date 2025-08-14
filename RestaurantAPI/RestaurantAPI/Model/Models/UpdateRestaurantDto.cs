using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Model.Models
{
    public record class UpdateRestaurantDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; init; } 
        public string Description { get; init; } 
        public string Category { get; init; } 
        public bool HasDelivery { get; init; }
    }
}
