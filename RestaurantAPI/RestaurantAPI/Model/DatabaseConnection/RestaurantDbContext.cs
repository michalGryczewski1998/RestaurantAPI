using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Model.Entities;

namespace RestaurantAPI.Model.DatabaseConnection
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        private readonly IConfiguration _configuration;

        public RestaurantDbContext(IConfiguration configuration) => _configuration = configuration;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration["ConnectionStrings:Default"];

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Dish>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
