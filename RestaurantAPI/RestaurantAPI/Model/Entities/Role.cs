namespace RestaurantAPI.Model.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public bool Aktualne { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
