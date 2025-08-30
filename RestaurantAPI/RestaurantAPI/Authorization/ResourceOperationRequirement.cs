using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Enums;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation { get; }

        public ResourceOperationRequirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
    }
}
