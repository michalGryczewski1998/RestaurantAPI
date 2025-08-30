using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Enums;
using RestaurantAPI.Model.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Restaurant resource)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read || 
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = Convert.ToInt32(context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

            if(resource.CreatedById == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
