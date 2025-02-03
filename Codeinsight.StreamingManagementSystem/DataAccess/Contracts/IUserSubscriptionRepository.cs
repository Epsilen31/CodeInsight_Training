using Codeinsight.StreamingManagementSystem.DataAccess.Entities;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Contracts
{
    public interface IUserSubscriptionRepository
    {
        void CreateSubscription(Subscription subscription);
        void UpdateSubscription(Subscription subscription);
        ICollection<Subscription> GetSubscriptionsByUserId(int userId);
    }
}
