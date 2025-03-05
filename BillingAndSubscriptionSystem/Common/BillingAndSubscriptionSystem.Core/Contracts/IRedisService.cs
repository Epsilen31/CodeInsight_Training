using BillingAndSubscriptionSystem.Core.TokenDatas;

namespace BillingAndSubscriptionSystem.Core.Contracts
{
    public interface IRedisService
    {
        Task SetTokenDataAsync(
            string key,
            TokenData tokenData,
            CancellationToken cancellationToken,
            TimeSpan? expiry = null
        );

        Task<TokenData?> GetTokenDataAsync(string key, CancellationToken cancellationToken);
    }
}
