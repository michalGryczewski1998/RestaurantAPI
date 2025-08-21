using FluentValidation;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Model.Validators
{
    public class CreateRestaurantValidator : AbstractValidator<CreateRestaurantDto>
    {
        public CreateRestaurantValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(50)
                .WithMessage("Nazwa jest wymagana, minimalna długość to trzy znaki ale nie większa niż 50 znaków !");
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50)
                .WithMessage("Opis jest wymagany, ale nie więcej niż 50 znaków !");
            RuleFor(x => x.Category)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50)
                .WithMessage("Kategoria jest wymagana, ale nie więcej niż 50 znaków !");
            RuleFor(x => x.HasDelivery)
                .NotNull()
                .WithMessage("Wymagana informacja o dostawie");
            RuleFor(x => x.ContactEmail)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Wymagany adres E-Mail");
            RuleFor(x => x.ContactNumber)
                .NotEmpty()             
                .Matches(@"^\+?[0-9]{9,15}$")
                .WithMessage("Niepoprawny format numeru telefonu.");
            RuleFor(x => x.City)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .WithMessage("Miasto jest wymagane");
            RuleFor(x => x.Street)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50)
                .WithMessage("Nazwa ulicy jest wymagana");
            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .NotNull()
                .Matches(@"^\d{2}-\d{3}$")
                .WithMessage("Kod pocztowy musi być w formacie 00-000.");
        }

    }
}
