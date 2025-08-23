using FluentValidation;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Model.Validators
{
    public class UpdateRestaurantValidator : AbstractValidator<UpdateRestaurantDto>
    {
        public UpdateRestaurantValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50)
                .WithMessage("Pole nazwy jest wymagane przy aktualizacji");
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(80)
                .WithMessage("Pole opisu jest wymagane przy aktualizacji");
            RuleFor(x => x.Category)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(40)
                .WithMessage("Pole opisu kategori jest wymagane przy aktualizacji");
            RuleFor(x => x.HasDelivery)
                .NotNull()
                .WithMessage("Pole dotyczące dostawy jest wymagane przy aktualizacji");
        }
    }
}
