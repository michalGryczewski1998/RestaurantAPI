using FluentValidation;
using RestaurantAPI.Model.Models;

namespace RestaurantAPI.Model.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = [5, 10, 15];
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
        }
    }
}
