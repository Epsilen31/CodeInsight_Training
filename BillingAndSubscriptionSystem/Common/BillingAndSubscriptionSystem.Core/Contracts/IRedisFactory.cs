using Microsoft.Extensions.Caching.Distributed;

namespace BillingAndSubscriptionSystem.Core.Contracts
{
    public interface IRedisFactory
    {
        IDistributedCache GetCache();
    }
}
