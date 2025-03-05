using BillingAndSubscriptionSystem.Services.DTOs;

namespace BillingAndSubscriptionSystem.Services.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(LoginDto user);
        bool VerifyToken(string token);
        string? GetClaimFromToken(string token, string claimType);
    }
}
