using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Model.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; } = default!;    
        public string ContactNumber { get; set; } = default!;
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public int AdressId { get; set; }
        public virtual Address Adress { get; set; }
        public virtual List<Dish> Dishes { get; set; }

        public void UpdateFrom(UpdateRestaurantDto dto)
        {
            Name = dto.Name;
            Description = dto.Description;
            Category = dto.Category;
            HasDelivery = dto.HasDelivery;
        }
    }
}
