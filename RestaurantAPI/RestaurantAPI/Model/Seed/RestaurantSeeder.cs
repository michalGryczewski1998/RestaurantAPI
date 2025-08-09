using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;

namespace RestaurantAPI.Model.Seed
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed() 
        {
            if (_dbContext.Database.CanConnect() && !_dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                _dbContext.Restaurants.AddRange(restaurants);
                _dbContext.SaveChanges();
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Description = "Kentaky Fried Chicken",
                    Category = "Fast food",
                    HasDelivery = true,
                    ContactEmail = "kfc@gmail.com",
                    ContactNumber = "+48987441265",
                    Dishes = new List<Dish>()
                    {
                        new Dish
                        {
                            Name = "Grander",
                            Description = "Chicken burger BBQ",
                            Price = 24.99M
                        },
                        new Dish
                        {
                            Name = "Twister",
                            Description = "Chicken in tortilla and BBQ",
                            Price = 17.99M
                        },
                        new Dish
                        {
                            Name = "Qritto",
                            Description = "Chicken quritto",
                            Price = 19.99M
                        },
                        new Dish
                        {
                            Name = "Longer",
                            Description = "Chicken in long bun",
                            Price = 12.99M
                        }
                    },
                    Adress = new Address
                    {
                        City = "Gdańsk",
                        Street = "Jakaśtamulicawgdanskugdziejestkfc",
                        PostalCode = "80-010"
                    }
                },

                new Restaurant()
                {
                    Name = "McDonalds",
                    Description = "Burger",
                    Category = "Fast food",
                    HasDelivery = true,
                    ContactEmail = "mcdonalds@gmail.com",
                    ContactNumber = "+41184442265",
                    Dishes = new List<Dish>()
                    {
                        new Dish
                        {
                            Name = "Big Mac",
                            Description = "Big burger with double meat",
                            Price = 26.99M
                        },
                        new Dish
                        {
                            Name = "McDouble",
                            Description = "Small burher with dwo slice meat",
                            Price = 7.99M
                        },
                        new Dish
                        {
                            Name = "McRoyall",
                            Description = "medium size burger with meat",
                            Price = 24.99M
                        },
                        new Dish
                        {
                            Name = "Chicker",
                            Description = "Chicken in long bun",
                            Price = 4.99M
                        }
                    },
                    Adress = new Address
                    {
                        City = "Gdańsk",
                        Street = "Jakaśtamulicawgdanskugdziejestkmak",
                        PostalCode = "80-010"
                    }
                }
            };
            
            return restaurants;
        }
    }
}
