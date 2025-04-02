using BillingAndSubscriptionSystem.DataAccess.Models;
using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IUserSubscriptionRepository
    {
        Task CreateUserSubscriptionAsync(
            Subscription userSubscription,
            CancellationToken cancellationToken
        );
        Task UpdateSubscriptionAsync(
            Subscription userSubscription,
            CancellationToken cancellationToken
        );
        Task<Subscription?> GetUserSubscriptionAsync(
            int userId,
            CancellationToken cancellationToken
        );
        Task<ICollection<Subscription>> GetAllUserSubscriptionsAsync(
            CancellationToken cancellationToken
        );
        Task<int> GetInactiveSubscriptionsCountAsync(CancellationToken cancellationToken);

        Task<ICollection<MonthlySubscriptionData>> GetMonthlySubscriptionsAsync(
            CancellationToken cancellationToken
        );
        Task<ICollection<PlanTypeCountData>> GetSubscriptionPlanCountsAsync(
            CancellationToken cancellationToken
        );

        // Remove Subscriptions
        Task<bool> DeleteUserSubscriptionAsync(
            int subscriptionId,
            CancellationToken cancellationToken
        );
    }
}
