using Microsoft.AspNetCore.Authorization;

namespace BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute.Requirement
{
    public class AdminRequirement : IAuthorizationRequirement
    {
        public string RequiredRole { get; }

        public AdminRequirement(string requiredRole = "Admin")
        {
            RequiredRole = requiredRole;
        }
    }
}
