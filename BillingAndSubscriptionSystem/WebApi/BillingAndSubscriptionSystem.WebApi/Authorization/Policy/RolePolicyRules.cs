using BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace BillingAndSubscriptionSystem.WebApi.Authorization.Policy
{
    public class RolePolicyRules
    {
        public const string ADMIN_ROLE = "Admin";

        public static void RegisterPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(
                ADMIN_ROLE,
                policy => policy.Requirements.Add(new AdminRequirement())
            );
        }
    }
}
