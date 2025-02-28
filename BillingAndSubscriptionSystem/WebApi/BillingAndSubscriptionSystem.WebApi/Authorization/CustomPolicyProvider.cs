using BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute.Requirement;
using BillingAndSubscriptionSystem.WebApi.Authorization.Policy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BillingAndSubscriptionSystem.WebApi.Authorization
{
    public class CustomPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _originalPolicyProvider;

        public CustomPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _originalPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return _originalPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return _originalPolicyProvider.GetFallbackPolicyAsync();
        }

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var existingPolicy = await _originalPolicyProvider.GetPolicyAsync(policyName);
            if (existingPolicy != null)
            {
                return existingPolicy;
            }

            if (policyName.Equals(RolePolicyRules.ADMIN_ROLE, StringComparison.OrdinalIgnoreCase))
            {
                var adminPolicy = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme
                )
                    .RequireAuthenticatedUser()
                    .AddRequirements(new AdminRequirement())
                    .Build();

                return adminPolicy;
            }

            return null;
        }
    }
}
