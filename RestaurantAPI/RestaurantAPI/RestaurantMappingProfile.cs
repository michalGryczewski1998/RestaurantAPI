using AutoMapper;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI
{
    // Profile mapowania
    public class RestaurantMappingProfile : Profile
    {
        /// <summary>
        /// Dla tego profilu można dodać mapowanie pozostałych właściwości
        /// ale JEŻELI typy i nazwy właściwości się pokrywają między tymi dwiema klasami 
        /// czyli Restaurant a RestaurantDto to AutoMapper automatycznie zmapuje te właściwości
        /// czyli ID, Name, Description, Category i HasDelivery się pokrywają
        /// ------------------------------------------------------------------------------------
        /// przykład -> .ForMember(c => c.Name, v => v.MapFrom(b => b.Name))
        /// </summary>
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(x => x.City, c => c.MapFrom(s => s.Adress.City))
                .ForMember(x => x.Street, c => c.MapFrom(s => s.Adress.Street))
                .ForMember(x => x.PostalCode, c => c.MapFrom(s => s.Adress.PostalCode));

            /// <summary>
            /// Typy i nazwy w tym wypadku zgadzają się więc taki zapis starczy
            /// </summary>
            CreateMap<Dish, DishDto>();

            /// <summary>
            /// Tworzymy nowy obiekt adresu
            /// </summary>
            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(r => r.Adress, c => c.MapFrom(dto => new Address()
                { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));

            /// <summary>
            /// Mapujemy z CreateDishDto na Dish
            /// Nie precyzujemy mapowania, z powodu pokrywania się pól
            /// </summary>
            CreateMap<CreateDishDto, Dish>();

        }
    }
}
