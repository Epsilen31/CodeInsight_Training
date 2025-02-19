namespace BillingAndSubscriptionSystem.Core.Contracts
{
    public interface IRedisService
    {
        Task SetValueAsync(
            string key,
            string value,
            CancellationToken cancellationToken,
            TimeSpan? expiry = null
        );
        Task<string> GetValueAsync(string key, CancellationToken cancellationToken);
    }
}
