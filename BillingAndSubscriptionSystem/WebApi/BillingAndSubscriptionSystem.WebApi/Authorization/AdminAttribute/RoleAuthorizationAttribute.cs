using Microsoft.AspNetCore.Authorization;

namespace BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute
{
    public class RoleAuthorizationAttribute : AuthorizeAttribute, IAuthorizationRequirement
    {
        public string Role { get; }

        public RoleAuthorizationAttribute(string role)
        {
            Role = role;
            Policy = $"{role}";
        }
    }
}
