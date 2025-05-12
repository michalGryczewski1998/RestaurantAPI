namespace RestaurantAPI.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }
        public string ContackEmail { get; set; } = default!;    
        public string ContacNumber { get; set; } = default!;
        public int AdressId { get; set; }
        public virtual Address Adress { get; set; }
        public virtual List<Dish> Dishes { get; set; }
    }
}
