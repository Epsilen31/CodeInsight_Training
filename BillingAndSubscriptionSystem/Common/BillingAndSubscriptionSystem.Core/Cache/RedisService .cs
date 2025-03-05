using System.Text.Json;
using BillingAndSubscriptionSystem.Core.Contracts;
using BillingAndSubscriptionSystem.Core.TokenDatas;
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

        public async Task SetTokenDataAsync(
            string key,
            TokenData tokenData,
            CancellationToken cancellationToken,
            TimeSpan? expiry = null
        )
        {
            var jsonData = JsonSerializer.Serialize(tokenData);

            var options = new DistributedCacheEntryOptions();
            if (expiry.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiry;
            }
            await _cache.SetStringAsync(key, jsonData, options, cancellationToken);
        }

        public async Task<TokenData?> GetTokenDataAsync(
            string key,
            CancellationToken cancellationToken
        )
        {
            var jsonData = await _cache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(jsonData))
                return null;

            return JsonSerializer.Deserialize<TokenData>(jsonData);
        }
    }
}
