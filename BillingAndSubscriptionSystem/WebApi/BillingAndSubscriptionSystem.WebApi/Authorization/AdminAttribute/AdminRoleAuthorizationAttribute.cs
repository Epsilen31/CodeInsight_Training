using Microsoft.AspNetCore.Authorization;

namespace BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute
{
    public class AdminRoleAuthorizationAttribute : AuthorizeAttribute
    {
        public AdminRoleAuthorizationAttribute(string policy)
        {
            Policy = policy;
        }
    }
}
