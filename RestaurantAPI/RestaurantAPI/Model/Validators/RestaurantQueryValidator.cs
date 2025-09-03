using FluentValidation;
using RestaurantAPI.Model.Entities;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Model.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = [5, 10, 15];
        private string[] allowedSortByColumns = [ nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description) ];
        public RestaurantQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize)
                .Custom((value, context) =>
                {
                    if (!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure("PageSize", $"Wielkość strony musi zawierać się w {string.Join(",",allowedPageSizes)}");
                    }
                })
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumns.Contains(value))
                .WithMessage($"SortBy jest opcjonalny lub musi zawierać się w {string.Join(", ",allowedSortByColumns)}");
        }
    }
}
