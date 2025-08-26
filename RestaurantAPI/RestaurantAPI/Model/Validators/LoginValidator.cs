using FluentValidation;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Model.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .WithMessage("Proszę podać adres E-Mail");
            RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(1)
            .WithMessage("Proszę podać hasło");
        }
    }
}
