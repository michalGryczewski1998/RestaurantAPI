using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Model.DatabaseConnection;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Model.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>, IWalidacjaUzytkownika
    {
        private readonly RestaurantDbContext _dbContext;

        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull()
                .Matches(@"^[\p{L}]+$") // tylko litery (Unicode)
                .WithMessage("Pole nie może być puste, i musi zawierać tylko litery.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .MinimumLength(6);
            RuleFor(x =>x.ConfirmPassword)
                .Equal(c => c.Password);
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{9,15}$")
                .WithMessage("Niepoprawny format numeru telefonu.");
            RuleFor(x => x.Nationality)
                .NotEmpty()
                .NotNull()
                .Matches(@"^[\p{L}]+$") // tylko litery (Unicode)
                .WithMessage("Pole nie może być puste, i musi zawierać tylko litery.");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .NotNull()
                .Must(c => 
                {
                    if(c == default) return false;

                    var dzisiaj = DateTime.Now;
                    var wiek = dzisiaj.Year - c.Year;

                    if (c.Date > dzisiaj.AddYears(-wiek)) wiek--;

                    return wiek >= 18;
                })
                .WithMessage("Musisz mieć co najmniej 18 lat.");

        }

        public List<Tuple<string, bool>> WalidacjaUzytkownika(User user)
        {
            var status = new List<Tuple<string, bool>>();

            var istniejeUzytkownikNaTenEmail = _dbContext.Users
                .Where(x => x.Email == user.Email);

            var istniejeUzytkownikNaTenNumerTelefonu = _dbContext.Users
                .Where(x => x.PhoneNumber == user.PhoneNumber);

            if (istniejeUzytkownikNaTenEmail.Any())
            {
                status.Add(
                    new Tuple<string, bool>($"Użytkownik {user.FirstName} musi podać inny adres E-Mail, podany adres {user.Email} jest używany !", true)
                    );
            }

            if (istniejeUzytkownikNaTenNumerTelefonu.Any())
            {
                status.Add(
                    new Tuple<string, bool>($"Użytkownik {user.FirstName} musi podać inny numer telefonu, podany numer {user.PhoneNumber} jest używany !", true)
                    );
            }

            return status;
        }
    }
}
