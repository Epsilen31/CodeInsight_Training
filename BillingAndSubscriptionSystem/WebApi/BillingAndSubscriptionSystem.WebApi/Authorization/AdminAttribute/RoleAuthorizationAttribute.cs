using Microsoft.AspNetCore.Authorization;

namespace BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute
{
    public class RoleAuthorizationAttribute
        : AuthorizeAttribute,
            IAuthorizationRequirement,
            IAuthorizationRequirementData
    {
        public string Role { get; }

        public RoleAuthorizationAttribute(string role)
        {
            Role = role;
            Policy = $"Admin{role}";
        }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            yield return this;
        }
    }
}
