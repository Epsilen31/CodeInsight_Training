using BillingAndSubscriptionSystem.Core.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace BillingAndSubscriptionSystem.Core.Cache
{
    public class RedisFactory : IRedisFactory
    {
        private readonly IDistributedCache _cache;

        public RedisFactory(IDistributedCache cache)
        {
            _cache = cache;
        }

        public IDistributedCache GetCache()
        {
            return _cache;
        }
    }
}
