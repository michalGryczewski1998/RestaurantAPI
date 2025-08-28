using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(x => x.Type == "DateOfBirth").Value);
            var userEmail = context.User.FindFirst(x => x.Type == ClaimTypes.Name).Value;

            if (DateTime.Today.Year - requirement.MinimumAge >= dateOfBirth.Year)
            {
                _logger.LogInformation($"Weryfikacja {context.User.Identity.Name} zakończona pomyślnie");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation($"Weryfikacja {context.User.Identity.Name} zakończona niepowodzeniem");
            }

            return Task.CompletedTask;
        }
    }
}
