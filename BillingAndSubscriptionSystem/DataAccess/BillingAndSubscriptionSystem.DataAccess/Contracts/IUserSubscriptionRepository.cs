using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IUserSubscriptionRepository
    {
        Task CreateUserSubscriptionAsync(Subscription userSubscription);
        Task UpdateSubscriptionAsync(Subscription userSubscription);
        Task<Subscription> GetUserSubscriptionAsync(int userId);
        Task<ICollection<Subscription>> GetAllUserSubscriptionsAsync();
    }
}
