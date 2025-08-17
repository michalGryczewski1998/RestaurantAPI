using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Model.Models
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public string Nationality { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int RoleId { get; set; } = 1;
    }
}
