namespace RestaurantAPI.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string PostalCode { get; set; } = default!;

        public virtual Restaurant Restaurant { get; set; }
    }
}
