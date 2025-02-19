using BillingAndSubscriptionSystem.Core.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace BillingAndSubscriptionSystem.Core.Cache
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cache;

        public RedisService(IRedisFactory redisFactory)
        {
            _cache = redisFactory.GetCache();
        }

        public async Task SetValueAsync(
            string key,
            string value,
            CancellationToken cancellationToken,
            TimeSpan? expiry = null
        )
        {
            var options = new DistributedCacheEntryOptions();
            if (expiry.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiry;
            }
            await _cache.SetStringAsync(key, value, options, cancellationToken);
        }

        public async Task<string> GetValueAsync(string key, CancellationToken cancellationToken)
        {
            return await _cache.GetStringAsync(key, cancellationToken) ?? string.Empty;
        }
    }
}
