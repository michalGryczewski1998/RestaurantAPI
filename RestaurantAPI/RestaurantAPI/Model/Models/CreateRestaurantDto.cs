using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Model.Models
{
    public class CreateRestaurantDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; } = default!;
        public string ContactNumber { get; set; } = default!;
        [Required]
        [MaxLength(50)]
        public string City { get; set; } = default!;
        [Required]
        [MaxLength(50)]
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}
