using System.Security.Claims;
using BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace BillingAndSubscriptionSystem.WebApi.Authorization.AdminAttribute.Handler
{
    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminRequirement requirement
        )
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                return Task.CompletedTask;
            }

            var adminClaim = context.User.FindFirst(claim =>
                claim.Type == ClaimTypes.Role
                && string.Equals(claim.Value, "Admin", StringComparison.OrdinalIgnoreCase)
            );

            if (adminClaim != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
