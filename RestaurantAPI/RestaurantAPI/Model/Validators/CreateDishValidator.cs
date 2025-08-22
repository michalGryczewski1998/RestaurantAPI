using FluentValidation;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Model.Validators
{
    public class CreateDishValidator : AbstractValidator<CreateDishDto>
    {
        public CreateDishValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(40)
                .WithMessage("Nazwa dania jest wymagana");
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(80)
                .WithMessage("Opis dania jest wymagany");
            RuleFor(x => x.Price)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Cena dania jest wymagana");
        }
    }
}
